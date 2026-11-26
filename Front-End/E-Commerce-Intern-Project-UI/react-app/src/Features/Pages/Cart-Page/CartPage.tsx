import { useEffect, useState } from "react";
import CartComponent from "../../Components/Cart/Cart-Component";
import "./CartPage.css";
import { CartDetailsResponse, InternoMicroserviceClient, ProductDetails_vw } from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../Core/Shared/API-URL";
import { useAuth } from "../../../Core/Services/AuthServices/AuthProvider";
import { getApiErrorMessage } from "../../../Core/Shared/getApiErrorMessage";
export default function CartPage() {
  const { user } = useAuth();
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [Products, SetProducts] = useState<ProductDetails_vw[]>([]);
  const [Cart, setCart] = useState<CartDetailsResponse>(new CartDetailsResponse());
  const [newProdutAdded, setnewProdutAdded] = useState<boolean>(false);

  const loadCartItems = async () => {
    try {
      const response = await Client.getUserCartItemsDetails();
      if (response.isSuccess) {
        console.log(response.data);

        setCart(response.data!);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

  const loadProd = async () => {
    try {
      const response = await Client.productDetailsVW();
      if (response.statusCode! > 200) {
        console.log(response.errorMessage);
      }
      if (response.isSuccess) {
        SetProducts(response.data!.slice(0, 3));
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  const loadProdWithFilter = async () => {
    SetProducts(
      Products.filter(
        (product) =>
          !Cart.cartItems?.find(
            (ct) => ct.product?.productID === product.productID,
          ),
      ),
    );
  };

  useEffect(() => {
    if (Products.length !== 0 && (Cart.cartItems?.length ?? 0) > 0) {
      loadProdWithFilter();
    }
  }, [Cart.cartItems]);
  useEffect(() => {
    if (Products.length === 0) {
      loadProd();
    }
  }, []);

  useEffect(() => {
    if (user && Cart) {
      loadCartItems();
    }
  }, [user]);

  async function onLoadingProductsEvent() {
    await loadCartItems();
    loadProdWithFilter();
  }
  function onFinishAddingProductToCartEvent() {
    setnewProdutAdded(false);
  }
  return (
    <>
      <div className="cart-container">
        <CartComponent
          isnewProductAdded={newProdutAdded}
          isloadingProducts={onLoadingProductsEvent}
          finishAddingNewProduct={onFinishAddingProductToCartEvent}
        />
      </div>
    </>
  );
}
