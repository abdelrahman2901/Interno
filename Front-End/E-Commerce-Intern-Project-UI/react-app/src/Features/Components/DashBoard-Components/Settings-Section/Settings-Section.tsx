import { useEffect, useState } from "react";
import AreaComponent from "./Area-Component/AreaComponent";
import CouponsComponent from "./Coupons-Component/CouponsComponent";
import CityComponent from "./City-Component/CityComponent";
import { AreaModel } from "../../../../Core/DTO/AreaDTO/AreaModel";
import { CityModel } from "../../../../Core/DTO/CityDTO/CityModel";
import { ShippingCostDetails } from "../../../../Core/DTO/ShippingCostDTO/ShippingCostDetails";
import { getAreas } from "../../../../Core/Services/AreaServices/AreaService";
import { getCities } from "../../../../Core/Services/CityServices/CityService";
import { GetAllShippingCostDetails } from "../../../../Core/Services/ShippingCostServices/ShippingCostServices";
import { SubSectionTypes } from "../../../../Core/Types/SubSectionTypes";

type props = {
  section: SubSectionTypes;
};
export default function SettingsSection({ section }: props) {
  const [areas, setAreas] = useState<AreaModel[]>([]);
  const [cities, setCities] = useState<CityModel[]>([]);
  const [shippingCosts, setShippingCosts] = useState<ShippingCostDetails[]>([]);
  const loadshippingCosts = async () => {
    try {
      const response = await GetAllShippingCostDetails();
      if (response.isSuccess) {
        setShippingCosts(response.data!);
      }
    } catch (err) {
      if (err) {
        console.error(err);
      }
    }
  };
  const loadcities = async () => {
    try {
      const response = await getCities();
      if (response.isSuccess) {
        setCities(response.data!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
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
    if (areas.length === 0) {
      loadAreas();
    }
    if (cities.length === 0) {
      loadcities();
    }
    if (shippingCosts.length === 0) {
      loadshippingCosts();
    }
  }, []);

  const [currentComponent, setCurrentComponent] =
    useState<SubSectionTypes>("Area");
  useEffect(() => {
    setCurrentComponent(section);
  }, [section]);
  function rendersection() {
    switch (currentComponent) {
      case "Area": {
        return (
          <AreaComponent
            areasProps={areas}
            cities={cities}
            shippingCosts={shippingCosts}
          />
        );
      }
      case "City": {
        return <CityComponent citiesProps={cities} areaProps={areas} />;
      }

      case "Coupon": {
        return <CouponsComponent />;
      }
      default: {
        return (
          <AreaComponent
            areasProps={areas}
            cities={cities}
            shippingCosts={shippingCosts}
          />
        );
      }
    }
  }
  return <>{rendersection()}</>;
}
