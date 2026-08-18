import React, { useEffect, useState } from "react";
import { AreaModel } from "../../../../../Core/DTO/AreaDTO/AreaModel";

import { CityModel } from "../../../../../Core/DTO/CityDTO/CityModel";
import { ShippingCostDetails } from "../../../../../Core/DTO/ShippingCostDTO/ShippingCostDetails";
import Add_UpdateAreaModel from "./Models/Add_UpdateAreaModel";
import {
  deleteArea,
  getAreas,
} from "../../../../../Core/Services/AreaServices/AreaService";
import DeleteAreaModel from "./Models/DeleteAreaModel";

type props = {
  areasProps: AreaModel[];
  cities: CityModel[];
  shippingCosts: ShippingCostDetails[];
};

export default function AreaComponent({
  cities,
  areasProps,
  shippingCosts,
}: props) {
  const [_areas, setPrivAreas] = useState<AreaModel[]>([]);
  const [areas, setAreas] = useState<AreaModel[]>([]);
  const [isAddingArea, setIsAddingArea] = useState<boolean>(false);
  const [isDeletingArea, setIsDeletingArea] = useState<boolean>(false);
  const [selectedAreaID, setSelectedAreaID] = useState<string | null>(null);
  const [selectedShingCostID, setSelectedShingCostID] = useState<string | null>(
    null,
  );
  
  const loadAreas = async () => {
    try {
      const response = await getAreas();
      if (response.isSuccess) {
        setAreas(response.data!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (_areas.length === 0) {
      setPrivAreas(areasProps);
    }
    if (areas.length === 0) {
      setAreas(areasProps);
    }
  }, [areasProps]);
  const deleteArea_fn = async () => {
    try {
      const response = await deleteArea(selectedAreaID!);
      if (response.isSuccess) {
        loadAreas();
      }
    } catch (err) {
      if (err) {
        console.error(err);
      }
    }
  };

  function extractCityName(cityID: string) {
    return cities.find((r) => r.cityID === cityID)?.cityName;
  }
  function extractShippingCostForAreaCount(areaID: string) {
    return shippingCosts.find((r) => r.area.areaID === areaID)?.shippingCost;
  }
  function extractAreaID(areaID: string) {
    return `Area-${areaID.slice(0, 3)}`;
  }

  function EditArea(areaID: string) {
    toggleModel();
    setSelectedAreaID(areaID);
    setSelectedShingCostID(
      shippingCosts.find((r) => r.area.areaID === areaID)?.shippingCostID!,
    );
  }

  function deleteAreaEvent(action: boolean) {
    if (action) {
      deleteArea_fn();
    }
    setIsDeletingArea(false);
  }
  async function DeleteArea(areaID: string) {
    setIsDeletingArea(true);
    setSelectedAreaID(areaID);
  }
  function toggleModel() {
    setIsAddingArea(true);
  }

  function onCloseModelEvent() {
    setSelectedShingCostID(null);
    setSelectedAreaID(null);
    setIsAddingArea(false);
    setIsDeletingArea(false);
  }
  function onFormSuccessModelEvent() {
    setIsAddingArea(false);
    loadAreas();
  }
  function filterAreas(e: React.FormEvent<HTMLSelectElement>) {
    const cityID = e.currentTarget.value;
    if (!cityID) {
      setAreas(_areas);

      return;
    }
    console.log(cityID);
    setAreas(_areas.slice().filter((r) => r.cityID === cityID));
  }
  return (
    <>
      <main className="main-content">
        <header className="top-header">
          <h2>Areas Management</h2>
          <button className="btn-primary" onClick={toggleModel}>
            + Add New Area
          </button>
        </header>

        <div className="content-section">
          <div className="filter-bar">
            <div className="filter-group">
              <label>Filter by City:</label>
              <select onChange={filterAreas}>
                <option value="">All Cities</option>
                {cities.map((city) => (
                  <option key={city.cityID} value={city.cityID}>
                    {city.cityName}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className="table-container">
            {areas.length > 0 && (
              <table className="data-table" id="areasTable">
                <thead>
                  <tr>
                    <th>Area ID</th>
                    <th>Area Name</th>
                    <th>City</th>
                    <th>Shipping Costs</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {areas.map((area) => (
                    <tr key={area.areaID}>
                      <td>
                        <span className="id-badge">
                          {extractAreaID(area.areaID)}
                        </span>
                      </td>
                      <td>
                        <strong>{area.areaName}</strong>
                      </td>
                      <td>
                        <span className="city-badge">
                          {extractCityName(area.cityID)}
                        </span>
                      </td>
                      <td>
                        <span className="count-badge">
                          {extractShippingCostForAreaCount(area.areaID)}
                        </span>
                      </td>
                      <td>
                        <button
                          className="btn-icon btn-edit"
                          onClick={() => EditArea(area.areaID)}
                          title="Edit"
                        >
                          ✏️
                        </button>
                        <button
                          className="btn-icon btn-delete"
                          onClick={() => DeleteArea(area.areaID)}
                          title="Delete"
                        >
                          🗑️
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}

            {areas.length === 0 && (
              <div className="empty-state" id="emptyState">
                <div className="empty-icon">📍</div>
                <h3>No Areas Found</h3>
                <p>Add your first area to get started</p>
                <button className="btn-primary" onClick={toggleModel}>
                  Add Area
                </button>
              </div>
            )}
          </div>
        </div>
      </main>
      {isAddingArea && (
        <Add_UpdateAreaModel
          onFormSuccess={onFormSuccessModelEvent}
          cities={cities}
          onClose={onCloseModelEvent}
          areaID={selectedAreaID}
          shippingCostID={selectedShingCostID}
        />
      )}
      {isDeletingArea && <DeleteAreaModel ConfirmResult={deleteAreaEvent} />}
    </>
  );
}
