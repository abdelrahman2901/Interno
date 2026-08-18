import React, { useEffect, useState } from "react";
import { AreaModel } from "../../../../../../Core/DTO/AreaDTO/AreaModel";
import { CityModel } from "../../../../../../Core/DTO/CityDTO/CityModel";
import {
  addArea,
  getAreaByID,
  updateArea,
} from "../../../../../../Core/Services/AreaServices/AreaService";
import { AreaRequest } from "../../../../../../Core/DTO/AreaDTO/AreaRequest";
import { ShippingCostRequest } from "../../../../../../Core/DTO/ShippingCostDTO/ShippingCostRequest";
import {
  CreateNewShippingCost,
  GetShippingCostDetailsByAreaID,
  UpdateShippingCost,
} from "../../../../../../Core/Services/ShippingCostServices/ShippingCostServices";
import { ShippingCostModel } from "../../../../../../Core/DTO/ShippingCostDTO/ShippingCostModel";

type props = {
  cities: CityModel[];
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

  const [area, setArea] = useState<AreaModel>(new AreaModel());
  const loadArea = async () => {
    try {
      const response = await getAreaByID(areaID!);
      if (response.isSuccess) {
        setArea(response.data!);
        console.log(area);
      }
    } catch (err) {
      if (err) {
        console.error(err);
      }
    }
  };
  const loadShippingCost = async () => {
    try {
      const response = await GetShippingCostDetailsByAreaID(areaID!);
      if (response.isSuccess) {
        setShippignCost(response.data?.shippingCost!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (areaID) {
      loadArea();
      loadShippingCost();
    }
  }, [areaID]);
  const updateArea_fn = async (updateAreaRequest: AreaModel) => {
    try {
      const response = await updateArea(updateAreaRequest);
      return response.isSuccess;
    } catch (err) {
      if (err) {
        console.error(err);
      }
    }
  };

  const addArea_fn = async (areaRequest: AreaRequest) => {
    try {
      const response = await addArea(areaRequest);
      if (response.isSuccess) {
        const shippingCostRequest: ShippingCostRequest = {
          araeID: response.data?.areaID!,
          shippingCost: shippingCost,
        };
        return shippingCostRequest;
      }
    } catch (err) {
      if (err) {
        console.error(err);
      }
    }
  };

  async function onSubmitForm(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const form = new FormData(e.currentTarget);
    setShippignCost(Number(form.get("ShippingCost") as string));

    if (areaID) //update
    {
      const updateAreaRequest: AreaModel = {
        areaID: areaID!,
        areaName: form.get("areaName") as string,
        cityID: form.get("cityID") as string,
      };

      const isAreaUpdated = await updateArea_fn(updateAreaRequest);

      if (shippingCost !== 0 && isAreaUpdated) {
        const updateShippingRequest: ShippingCostModel = {
          areaID: areaID,
          shippingCost: shippingCost,
          isDeleted: false,
          shippingCostID: shippingCostID!,
        };

        try {
          console.log(updateShippingRequest);
          const response = await UpdateShippingCost(updateShippingRequest);
          if (response.isSuccess) {
            onFormSuccess();
          }
        } catch (err) {
          if (err) {
            console.error(err);
          }
        }
      }
    } else //add new
    {
      try {
        const areaRequest: AreaRequest = {
          areaName: form.get("areaName") as string,
          cityID: form.get("cityID") as string,
        };
        const shippingREquest = await addArea_fn(areaRequest);
        try {
          const response = await CreateNewShippingCost(shippingREquest!);
          if (response.isSuccess) {
            onFormSuccess();
          }
        } catch (err) {
          if (err) {
            console.error(err);
          }
        }
      } catch (err) {
        if (err) {
          console.error(err);
        }
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
                    setArea((prev) => ({ ...prev, cityID: cityID }));
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
                    setArea((prev) => ({ ...prev, areaName: areaName }));
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
