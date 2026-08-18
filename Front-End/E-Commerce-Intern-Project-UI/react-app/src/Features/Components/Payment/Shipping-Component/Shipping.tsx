import { useEffect, useState } from "react";
import { AddressDetails } from "../../../../Core/DTO/AddressDTO/AddressDetails";
import { GetUserAddresess } from "../../../../Core/Services/AddressServices/AddressService";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
import { Link } from "react-router-dom";
import { PaymentCheckOutEnum } from "../../../../Core/Enums/PaymentCheckOutEnum";

type props = {
  onSelectingAddress: (value: AddressDetails) => void;
  onRedirectToPayment: (value: PaymentCheckOutEnum) => void;
};
export default function Shipping({
  onSelectingAddress,
  onRedirectToPayment,
}: props) {
  const [addresses, setAddressses] = useState<AddressDetails[]>([]);
  const [selectedAddresses, setSelectedAddresses] = useState<string>("");
  const { user } = useAuth();
  const loadAddress = async () => {
    try {
      const response = await GetUserAddresess(user?.userID!);
      if (response.isSuccess) {
        setAddressses(response.data!);
        setActiveAddress(response.data?.find((r) => r.isDefault)!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  useEffect(() => {
    if (addresses.length === 0 && user) {
      loadAddress();
    }
  }, [user]);

  function setActiveAddress(address: AddressDetails) {
    onSelectingAddress(address);
    setSelectedAddresses(address.addressID);
  }
  return (
    <>
      {/* <div className="checkout-container"> */}
      {addresses.length > 0 && (
        <div className="checkout-content">
          <h1 className="page-title">Shipping Information</h1>

          <div className="addresses-section">
            <div className="section-header">
              <h2>Select Shipping Address</h2>
              {/* <button className="btn-add-address" id="addNewAddressBtn">
                + Add New Address
              </button> */}
            </div>

            <div className="saved-addresses">
              <>
                {addresses.map((address) => (
                  <div
                    className="address-card"
                    onClick={() => setActiveAddress(address)}
                    key={address.addressID}
                  >
                    <input
                      onChange={() => {}}
                      type="radio"
                      name="selectedAddress"
                      checked={
                        selectedAddresses === address.addressID ||
                        address.isDefault
                      }
                    />
                    <label>
                      {address.isDefault && (
                        <div className="address-badge default-badge">
                          Default
                        </div>
                      )}
                      <div className="address-header">
                        <h3>{address.addressLabel}</h3>
                        {/* <button className="btn-edit" onclick="editAddress(1)">Edit</button> */}
                      </div>
                      <div className="address-details">
                        <p className="address-name">{user?.personName}</p>
                        <p>1- {address.mainAddress}</p>
                        {address.backUpAddress && (
                          <p>2- {address.backUpAddress}</p>
                        )}
                        <p>
                          {address.cityName} - {address.areaName}
                        </p>

                        <p className="address-phone">📞 {user?.phoneNumber}</p>
                        {address.backUpPhoneNumber && (
                          <p className="address-phone">
                            📞 {address.backUpPhoneNumber}
                          </p>
                        )}
                      </div>
                    </label>
                  </div>
                ))}
              </>
            </div>
          </div>

          <div className="checkout-actions">
            <button
              className="btn-continue"
              onClick={() => onRedirectToPayment(PaymentCheckOutEnum.Payment)}
            >
              Continue to Payment →
            </button>
          </div>
        </div>
      )}
      {addresses.length === 0 && (
        <div className="empty-state">
          <div className="empty-state-icon">📁</div>
          <h4>Looks Like You Didnt Provide Any Address Information</h4>
          <button className="btn-place-order">
            <Link to="/Home/ManageAccount?section=Address" className="nav-link">
              Go Add Your Addresses Now
            </Link>
          </button>
        </div>
      )}
    </>
  );
}
