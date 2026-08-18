import { useEffect, useState } from "react";
import "./addresses.css";
import AddAddressModel from "./Model/AddAddressModel";
import { AddressDetails } from "../../../../Core/DTO/AddressDTO/AddressDetails";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
import {
  DeleteAddress_serv,
  GetUserAddresess,
} from "../../../../Core/Services/AddressServices/AddressService";

export default function AccountAddress() {
  const [isAddingNewAddress, SetisAddingNewAddress] = useState(false);
  const [userAddress, setUserAddress] = useState<AddressDetails[]>([]);
  const [selectedAddressID, setSelectedAddressID] = useState<string | null>(
    null,
  );
  const { user } = useAuth();

  const loadAddresses = async () => {
    try {
      const response = await GetUserAddresess(user?.userID!);
      if (response.isSuccess) {
        setUserAddress(response.data!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const DeleteAddress_API = async (addressID: string) => {
    try {
      const response = await DeleteAddress_serv(addressID);
      if (response.isSuccess) {
        loadAddresses();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  function onCloseAddressModelEvent() {
    SetisAddingNewAddress(false);
    setSelectedAddressID(null);
  }
  function onAddingAddressEvent() {
    loadAddresses();
    SetisAddingNewAddress(false);
    setSelectedAddressID(null);
  }
  function DeleteAddress(addressID: string) {
    DeleteAddress_API(addressID);
  }

  useEffect(() => {
    loadAddresses();
  }, []);

  return (
    <>
      <h2 className="section-title">
        Saved Addresses
        <button
          className="Account-btn-primary Account-btn-small"
          onClick={() => SetisAddingNewAddress(true)}
        >
          + Add New Address
        </button>
      </h2>

      <div className="Account-addresses-grid">
        {userAddress.map((address) => (
          <div key={address.addressID} className="Account-address-card">
            <div className="Account-address-header">
              {address.isDefault && (
                <span className="Account-badge-primary">Default</span>
              )}
              <div className="Account-address-actions">
                <button
                  className="Account-icon-btn"
                  title="Edit"
                  onClick={() => {
                    setSelectedAddressID(address.addressID);
                    SetisAddingNewAddress(true);
                  }}
                >
                  ✏️
                </button>
                <button
                  className="Account-icon-btn"
                  title="Delete"
                  onClick={() => DeleteAddress(address.addressID)}
                >
                  🗑️
                </button>
              </div>
            </div>
            <h4>{address.addressLabel}</h4>
            <p>{user?.personName}</p>
            <p>{address.mainAddress}</p>
            {address.backUpAddress && <p>{address.backUpAddress}</p>}
            <p>
              {address.cityName} - {address.areaName}
            </p>
            <p>Phone:{user?.phoneNumber} </p>
            {address.backUpPhoneNumber && (
              <p>BackUp Phone:{address.backUpPhoneNumber} </p>
            )}
          </div>
        ))}
        {/* 
        <div className="Account-address-card">
          <div className="Account-address-header">
            <span className="Account-badge-primary">Default</span>
            <div className="Account-address-actions">
              <button
                className="Account-icon-btn"
                title="Edit"
                onClick={() => SetisAddingNewAddress(true)}
              >
                ✏️
              </button>
              <button className="Account-icon-btn" title="Delete">
                🗑️
              </button>
            </div>
          </div>
          <h4>Home</h4>
          <p>John Doe</p>
          <p>123 Main Street, Apt 4B</p>
          <p>New York, NY 10001</p>
          <p>United States</p>
          <p>Phone: +1 (555) 123-4567</p>
        </div> */}
      </div>
      {isAddingNewAddress && (
        <AddAddressModel
          onClose={onCloseAddressModelEvent}
          AddressID={selectedAddressID}
          onAddingNewAddress={onAddingAddressEvent}
        />
      )}
    </>
  );
}
