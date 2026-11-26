import React, { useEffect, useState } from "react";
import { AreaRequest, AreaResponse, AreaUpdateRequest, CityResponse, CreateShippingCostRequest, InternoMicroserviceClient, UpdateShippingCostRequest } from "../../../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../../../../Core/Shared/getApiErrorMessage";
type props = {
  cities: CityResponse[];
  onClose: () => void;
  areaID: string | null;
  shippingCostID: string | null;
  onFormSuccess: () => void;
};

export default function Add_UpdateAreaModel({
  cities,
  onClose,
  areaID,
  shippingCostID,
  onFormSuccess,
}: props) {
  const [shippingCost, setShippignCost] = useState<number>(0);

  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [area, setArea] = useState<AreaResponse>(new AreaResponse());
  const loadArea = async () => {
    try {
      const response = await Client.getAreaByID(areaID!);
      if (response.isSuccess) {
        setArea(response.data!);
        console.log(area);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  const loadShippingCost = async () => {
    try {
      const response = await Client.getShippingCostDetailsByAreaID(areaID!);
      if (response.isSuccess) {
        setShippignCost(response.data?.shippingCost!);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  useEffect(() => {
    if (areaID) {
      loadArea();
      loadShippingCost();
    }
  }, [areaID]);
  const updateArea_fn = async (updateAreaRequest: AreaUpdateRequest) => {
    try {
      const response = await Client.putArea(updateAreaRequest);
      return response.isSuccess;
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

  const addArea_fn = async (areaRequest: AreaRequest) => {
    try {
      const response = await Client.postArea(areaRequest);
      if (response.isSuccess) {
        return new CreateShippingCostRequest({
          araeID: response.data?.areaID!,
          shippingCost: shippingCost,
        });
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

  async function onSubmitForm(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const form = new FormData(e.currentTarget);
    setShippignCost(Number(form.get("ShippingCost") as string));

    if (areaID) //update
    {
      const updateAreaRequest = new AreaUpdateRequest({
        areaID: areaID!,
        areaName: form.get("areaName") as string,
        cityID: form.get("cityID") as string,
      });

      const isAreaUpdated = await updateArea_fn(updateAreaRequest);

      if (shippingCost !== 0 && isAreaUpdated) {
        const updateShippingRequest = new UpdateShippingCostRequest({
          areaID: areaID,
          shippingCost: shippingCost,
          isDeleted: false,
          shippingCostID: shippingCostID!,
        });

        try {
          console.log(updateShippingRequest);
          const response = await Client.updateShippingCost(updateShippingRequest);
          if (response.isSuccess) {
            onFormSuccess();
          }
        } catch (err) {
          console.error(getApiErrorMessage(err));
        }
      }
    } else //add new
    {
      try {
        const areaRequest = new AreaRequest({
          areaName: form.get("areaName") as string,
          cityID: form.get("cityID") as string,
        });
        const shippingREquest = await addArea_fn(areaRequest);
        try {
          const response = await Client.shippingCostsPOST(shippingREquest!);
          if (response.isSuccess) {
            onFormSuccess();
          }
        } catch (err) {
          console.error(getApiErrorMessage(err));
        }
      } catch (err) {
        console.error(getApiErrorMessage(err));
      }
    }
  }
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content">
          <div className="Custom-modal-header">
            <h3>Add New Area</h3>
            <button className="modal-close" onClick={onClose}>
              &times;
            </button>
          </div>
          <form onSubmit={onSubmitForm}>
            <div className="Custom-modal-body">
              <div className="Custom-form-group">
                <label>City *</label>
                <select
                  name="cityID"
                  onChange={(e) => {
                    e.preventDefault();
                    const cityID = e.currentTarget.value;
                    setArea(
                      (prev) => new AreaResponse({ ...prev, cityID: cityID }),
                    );
                  }}
                  value={area.cityID}
                >
                  <option value="">Select City</option>
                  {cities.map((city) => (
                    <option key={city.cityID} value={city.cityID}>
                      {city.cityName}
                    </option>
                  ))}
                </select>
              </div>

              <div className="Custom-form-group">
                <label>Area Name *</label>
                <input
                  name="areaName"
                  type="text"
                  required
                  placeholder="Enter area name"
                  onChange={(e) => {
                    e.preventDefault();
                    const areaName = e.currentTarget.value;
                    setArea(
                      (prev) =>
                        new AreaResponse({ ...prev, areaName: areaName }),
                    );
                  }}
                  value={area.areaName}
                />
              </div>
              <div className="Custom-form-group">
                <label>Shipping Cost *</label>
                <input
                  name="ShippingCost"
                  type="number"
                  required
                  placeholder="Enter Shipping Cost Price "
                  onChange={(e) => {
                    e.preventDefault();
                    const ShippingCost = e.currentTarget.value;
                    setShippignCost(Number(ShippingCost));
                  }}
                  value={shippingCost}
                />
              </div>
            </div>
            <div className="Custom-modal-footer">
              <button type="button" className="btn-secondary" onClick={onClose}>
                Cancel
              </button>
              <button type="submit" className="btn-primary">
                Save Area
              </button>
            </div>
          </form>
        </div>
      </div>
    </>
  );
}
