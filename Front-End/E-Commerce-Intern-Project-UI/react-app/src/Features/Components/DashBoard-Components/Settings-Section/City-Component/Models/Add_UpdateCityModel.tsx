import { useEffect, useState } from "react";
import {
  addCity,
  getCities,
  getCityByID,
  updateCity_serv,
} from "../../../../../../Core/Services/CityServices/CityService";
import { CityModel } from "../../../../../../Core/DTO/CityDTO/CityModel";
import { CityRequest } from "../../../../../../Core/DTO/CityDTO/CityRequest";

type props = {
  cityIDProps: string | null;
  onCloseModel: () => void;
  onReloadCities: () => void;
};

export default function Add_UpdateCityModel({
  cityIDProps,
  onCloseModel,
  onReloadCities,
}: props) {
  const [city, setCity] = useState<CityModel>(new CityModel());
  const [error, setError] = useState<string>("");
  const loadCityDetails = async () => {
    try {
      const response = await getCityByID(cityIDProps!);
      if (response.isSuccess) {
        setCity(response.data!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (cityIDProps) {
      loadCityDetails();
    }
  }, [cityIDProps]);

  async function addNewCity() {
    try {
      const response = await addCity({ cityName: city.cityName });
      if (!response.isSuccess && response.errorMessage) {
        setError(response.errorMessage!);
        return;
      }
      onReloadCities();
    } catch (err) {
      if (err) console.error(err);
    }
  }
  async function updateCity() {
    try {
      const response = await updateCity_serv(city);
      if (!response.isSuccess && response.errorMessage) {
        setError(response.errorMessage!);
        return;
      }
      onReloadCities();
    } catch (err) {
      if (err) console.error(err);
    }
  }
  function SaveCity() {
    if (cityIDProps) {
      updateCity();
    } else {
      addNewCity();
    }
  }
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content">
          <div className="Custom-modal-header">
            <h3>Add New City</h3>
            <button className="modal-close" onClick={onCloseModel}>
              &times;
            </button>
          </div>
          <form>
            <div className="Custom-modal-body">
              <div className="Custom-form-group">
                <label>City Name *</label>
                <input
                  type="text"
                  placeholder="Enter city name"
                  value={city.cityName}
                  onChange={(e) => {
                    const cityName = e.currentTarget.value;
                    setCity((prev) => ({ ...prev, cityName: cityName }));
                  }}
                />
                <span className="error">{error}</span>
              </div>
            </div>
            <div className="Custom-modal-footer">
              <button
                type="button"
                className="btn-secondary"
                onClick={onCloseModel}
              >
                Cancel
              </button>
              <button type="button" className="btn-primary" onClick={SaveCity}>
                Save City
              </button>
            </div>
          </form>
        </div>
      </div>
    </>
  );
}
