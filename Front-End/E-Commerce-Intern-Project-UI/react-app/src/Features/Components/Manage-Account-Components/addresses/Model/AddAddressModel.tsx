import React, { useEffect, useState } from "react";
import "../../Shared/CSS/ModelCSS.css";
import { useAuth } from "../../../../../Core/Services/AuthServices/AuthProvider";
import { AddressDetails, AreaResponse, CityResponse, CreateAddressRequest, InternoMicroserviceClient } from "../../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../../../Core/Shared/getApiErrorMessage";
type props = {
  onClose: () => void;
  AddressID?: string | null;
  onAddingNewAddress: () => void;
};
export default function AddAddressModel({
  onClose,
  AddressID,
  onAddingNewAddress,
}: props) {
  const { user } = useAuth();
  const [address, SetAddress] = useState<AddressDetails>(new AddressDetails());
  const [cities, setCities] = useState<CityResponse[]>([]);
  const [_areas, setPrivAreas] = useState<AreaResponse[]>([]);
  const [areas, setAreas] = useState<AreaResponse[]>([]);
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const loadCities = async () => {
    try {
      const response = await Client.getAllCities();
      if (response.isSuccess) {
        setCities(response.data!);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  const loadAreas = async () => {
    try {
      const response = await Client.getAllAreas();
      if (response.isSuccess) {
        setPrivAreas(response.data!);
        setAreas(_areas);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  useEffect(() => {
    console.log("Address changed:", address);
  }, [address]);
  const loadAddress = async () => {
    try {
      const response = await Client.getUserAddressByID(AddressID!);
      if (response.isSuccess) {
        console.log(response.data);

        SetAddress(response.data!);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

  useEffect(() => {
    loadCities();
    loadAreas();
  }, []);
  useEffect(() => {
    const filteredAreas = _areas
      .slice()
      .filter((r) => r.cityID === address.cityID);

    setAreas(filteredAreas);
  }, [address]);
  useEffect(() => {
    console.log(AddressID);

    if (AddressID) {
      loadAddress();
    }
  }, [AddressID]);

  function OnFormSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const addAddressRequest = async (addrequest: CreateAddressRequest) => {
      try {
        const response = await Client.createAddress(addrequest);
        if (response.isSuccess) {
          onAddingNewAddress();
        }
      } catch (err) {
        console.error(getApiErrorMessage(err));
      }
    };
    const updateAddressRequest = async () => {
      try {
        const response = await Client.updateAddress(address);
        if (response.isSuccess) {
          onAddingNewAddress();
        }
      } catch (err) {
        console.error(getApiErrorMessage(err));
      }
    };
    const form = new FormData(e.currentTarget);
    if (address.addressID === "") {
      const addrequest: CreateAddressRequest = new CreateAddressRequest({
        cityID: form.get("cityID") as string,
        addressLabel: form.get("addressLabel") as string,
        areaID: form.get("areaID") as string,
        mainAddress: form.get("mainAddress") as string,
        backUpAddress: form.get("backUpAddress") as string,
        backUpPhoneNumber: form.get("backUpPhoneNumber") as string,
        isDefault: (form.get("isDefault") as string) === "on" ? true : false,
      });
      console.log("adding new ");
      console.log(addrequest);

      addAddressRequest(addrequest);
    } else {
      console.log("updateing ");
      console.log(address);

      updateAddressRequest();
    }
  }
  function filterAreas(value: React.ChangeEvent<HTMLSelectElement>) {
    try {
      const cityID = value.currentTarget.value;

      const filteredAreas = _areas.slice().filter((r) => r.cityID === cityID);

      setAreas(filteredAreas);
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  }
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content">
          <div className="Custom-modal-header">
            <h3>Add New Address</h3>
            <button className="close-btn" onClick={onClose}>
              &times;
            </button>
          </div>
          <form onSubmit={OnFormSubmit}>
            <div className="form-group">
              <label>Address Label</label>
              <input
                type="text"
                name="addressLabel"
                required
                placeholder="e.g., Home, Office"
                onChange={(e) => {
                  const addresslabel = e.currentTarget.value;
                  SetAddress(
                    (prev: AddressDetails) =>
                      new AddressDetails({
                        ...prev,
                        addressLabel: addresslabel,
                      }),
                  );
                }}
                defaultValue={
                  address?.addressLabel ? address?.addressLabel : ""
                }
              />
            </div>

            <div className="form-group">
              <label>Main Address Line </label>
              <input
                type="text"
                name="mainAddress"
                required
                placeholder="Street address"
                onChange={(e) => {
                  const address = e.currentTarget.value;
                  SetAddress(
                    (prev: AddressDetails) =>
                      new AddressDetails({
                        ...prev,
                        mainAddress: address,
                      }),
                  );
                }}
                defaultValue={address?.mainAddress ? address?.mainAddress : ""}
              />
            </div>

            <div className="form-group">
              <label>BackUp Address Line (Optional)</label>
              <input
                type="text"
                name="backUpAddress"
                placeholder="Apartment, suite, etc."
                onChange={(e) => {
                  const address = e.currentTarget.value;
                  SetAddress(
                    (prev: AddressDetails) =>
                      new AddressDetails({
                        ...prev,
                        backUpAddress: address,
                      }),
                  );
                }}
                defaultValue={
                  address?.backUpAddress ? address?.backUpAddress : ""
                }
              />
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>City</label>
                <select
                  required
                  name="cityID"
                  onChange={(e) => {
                    debugger;
                    const cityID = e.currentTarget.value;
                    // console.log(e.currentTarget.value);

                    filterAreas(e); //fix send id instead
                    SetAddress(
                      (prev: AddressDetails) =>
                        new AddressDetails({
                          ...prev,
                          cityID: cityID,
                          // cityID: e.currentTarget.value,
                        }),
                    );
                    console.log(address);
                  }}
                  value={address?.cityID ?? ""}
                >
                  <option value="">Select City</option>
                  {cities.map((city) => (
                    <option key={city.cityID} value={city.cityID}>
                      {city.cityName}
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Area</label>
                <select
                  required
                  name="areaID"
                  onChange={(e) => {
                    console.log(address);
                    const areaID = e.currentTarget.value;
                    SetAddress(
                      (prev: AddressDetails) =>
                        new AddressDetails({
                          ...prev,
                          areaID: areaID,
                        }),
                    );
                    console.log(address);
                  }}
                  value={address?.areaID ?? ""}
                >
                  <option value="">Select Area</option>
                  {areas.map((area) => (
                    <option key={area.areaID} value={area.areaID}>
                      {area.areaName}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="form-group">
              <label>BackUp Phone Number (Optional)</label>
              <input
                type="tel"
                name="backUpPhoneNumber"
                placeholder="+1 (555) 000-0000"
                onChange={(e) => {
                  const phone = e.currentTarget.value;
                  SetAddress(
                    (prev: AddressDetails) =>
                      new AddressDetails({
                        ...prev,
                        backUpPhoneNumber: phone,
                      }),
                  );
                }}
                defaultValue={
                  address?.backUpPhoneNumber ? address?.backUpPhoneNumber : ""
                }
              />
            </div>

            <div className="form-group">
              <label>Set as default address</label>
              <input
                type="checkbox"
                name="isDefault"
                onChange={(e) => {
                  const isChecked = e.currentTarget.checked;
                  SetAddress(
                    (prev: AddressDetails) =>
                      new AddressDetails({
                        ...prev,
                        isDefault: isChecked,
                      }),
                  );
                }}
                checked={address?.isDefault}
              />
            </div>

            <div className="form-actions">
              <button
                type="button"
                className="btn-secondary"
                onClick={() => {
                  onClose();
                }}
              >
                Cancel
              </button>
              <button type="submit" className="btn-primary">
                Save Address
              </button>
            </div>
          </form>
        </div>
      </div>
    </>
  );
}
