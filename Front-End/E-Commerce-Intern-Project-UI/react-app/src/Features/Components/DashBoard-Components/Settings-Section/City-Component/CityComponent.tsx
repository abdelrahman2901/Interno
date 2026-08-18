import { useEffect, useState } from "react";
import { CityModel } from "../../../../../Core/DTO/CityDTO/CityModel";
import {
  deleteCity,
  getCities,
} from "../../../../../Core/Services/CityServices/CityService";
import { AreaModel } from "../../../../../Core/DTO/AreaDTO/AreaModel";
import DeleteCityModel from "./Models/DeleteCityMode";
import Add_UpdateCityModel from "./Models/Add_UpdateCityModel";

type props = {
  citiesProps: CityModel[];
  areaProps: AreaModel[];
};

export default function CityComponent({ citiesProps, areaProps }: props) {
  const [cities, setCities] = useState<CityModel[]>([]);
  const [areas, setAreas] = useState<AreaModel[]>([]);
  const [isOpenModel, setIsOpenModel] = useState<boolean>(false);
  const [isDeletingCity, setIsDeletingCity] = useState<boolean>(false);
  const [selectedCityID, setSelectedCityID] = useState<string | null>(null);
  useEffect(() => {
    if (cities.length === 0) {
      setCities(citiesProps);
    }
    if (areas.length === 0) {
      setAreas(areaProps);
    }
  }, [areaProps, citiesProps]);

  const loadCities = async () => {
    try {
      const response = await getCities();
      if (response.isSuccess) {
        setCities(response.data!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  function extractCityID(CityID: string) {
    return `City-${CityID.slice(0, 3)}`;
  }
  function extractCityAreasCount(CityID: string) {
    return areas.slice().filter((r) => r.cityID === CityID).length;
  }
  function EditCity(cityID: string) {
    setSelectedCityID(cityID);
    setIsOpenModel(true);
  }
  async function DeleteCity() {
    try {
      const response = await deleteCity(selectedCityID!);
      if (response.isSuccess) {
        setIsDeletingCity(false);
        loadCities();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }

  function onDeleteActionResultEvent(result: boolean) {
    if (result) {
      console.log(selectedCityID);

      DeleteCity();
    }
    setIsDeletingCity(false);
  }
  function toggleDeleteModel() {
    setIsDeletingCity(!isDeletingCity);
  }
  function toggleAdd_UpdateModel() {
    setIsOpenModel(!isOpenModel);
    if (isOpenModel === false) {
      setSelectedCityID(null);
    }
  }
  function reLoadCitiesEvent() {
    toggleAdd_UpdateModel();
    loadCities();
  }

  return (
    <>
      <main className="main-content">
        <header className="top-header">
          <h2>Cities Management</h2>
          <button className="btn-primary" onClick={toggleAdd_UpdateModel}>
            + Add New City
          </button>
        </header>

        <div className="content-section">
          <div className="table-container">
            <table className="data-table">
              <thead>
                <tr>
                  <th>City ID</th>
                  <th>City Name</th>
                  <th>Number of Areas</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {cities.map((city) => (
                  <tr key={city.cityID}>
                    <td>
                      <span className="id-badge">
                        {extractCityID(city.cityID)}
                      </span>
                    </td>
                    <td>
                      <strong>{city.cityName}</strong>
                    </td>
                    <td>
                      <span className="count-badge">
                        {extractCityAreasCount(city.cityID)} areas
                      </span>
                    </td>
                    <td>
                      <button
                        className="btn-icon btn-edit"
                        onClick={() => {
                          EditCity(city.cityID);
                        }}
                        title="Edit"
                        type="button"
                      >
                        ✏️
                      </button>
                      <button
                        type="button"
                        className="btn-icon btn-delete"
                        onClick={() => {
                          setSelectedCityID(city.cityID);
                          toggleDeleteModel();
                        }}
                        title="Delete"
                      >
                        🗑️
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            {cities.length === 0 && (
              <div className="empty-state" id="emptyState">
                <div className="empty-icon">🏙️</div>
                <h3>No Cities Found</h3>
                <p>Add your first city to get started</p>
                <button className="btn-primary" onClick={toggleAdd_UpdateModel}>
                  Add City
                </button>
              </div>
            )}
          </div>
        </div>
      </main>
      {isOpenModel && (
        <Add_UpdateCityModel
          onCloseModel={toggleAdd_UpdateModel}
          onReloadCities={reLoadCitiesEvent}
          cityIDProps={selectedCityID}
        />
      )}
      {isDeletingCity && (
        <DeleteCityModel onActionResult={onDeleteActionResultEvent} />
      )}
    </>
  );
}
