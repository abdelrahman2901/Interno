import { useEffect, useState } from "react";
import { CityRequest, CityResponse, CityUpdateRequest, InternoMicroserviceClient } from "../../../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../../../../Core/Shared/getApiErrorMessage";
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
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [city, setCity] = useState<CityResponse>(new CityResponse());
  const [error, setError] = useState<string>("");
  const loadCityDetails = async () => {
    try {
      const response = await Client.cities(cityIDProps!);
      if (response.isSuccess) {
        setCity(response.data!);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  useEffect(() => {
    if (cityIDProps) {
      loadCityDetails();
    }
  }, [cityIDProps]);

  async function addNewCity() {
    try {
      const response = await Client.postCity(
        new CityRequest({ cityName: city.cityName }),
      );
      if (!response.isSuccess && response.errorMessage) {
        setError(response.errorMessage!);
        return;
      }
      onReloadCities();
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  }
  async function updateCity() {
    try {
      const response = await Client.putCity(
        new CityUpdateRequest({
          cityID: city.cityID,
          cityName: city.cityName,
        }),
      );
      if (!response.isSuccess && response.errorMessage) {
        setError(response.errorMessage!);
        return;
      }
      onReloadCities();
    } catch (err) {
      console.error(getApiErrorMessage(err));
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
                    setCity(
                      (prev) =>
                        new CityResponse({ ...prev, cityName: cityName }),
                    );
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
