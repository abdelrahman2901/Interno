import { useEffect, useState } from "react";
import { GetProducts } from "../../../Core/Services/ProductServices/ProductServiceQuery";
import CartComponent from "../../Components/Cart/Cart-Component";
import "./CartPage.css";
import { IProductDetails } from "../../../Core/Interface/Products/IProductDetails";
import { AddCartItem } from "../../../Core/Services/Cart-Item-Services/CartItemService";
import { useAuth } from "../../../Core/Services/AuthServices/AuthProvider";
import { CartDetails } from "../../../Core/DTO/CartDTO/CartDetails";
import { GetUserCartItemsDetails } from "../../../Core/Services/CartServices/CartService";

export default function CartPage() {
  const Base_Url = "https://localhost:7164";
  const { user } = useAuth();
  const [Products, SetProducts] = useState<IProductDetails[]>([]);
  const [Cart, setCart] = useState<CartDetails>(new CartDetails());
  const [newProdutAdded, setnewProdutAdded] = useState<boolean>(false);

  const loadCartItems = async () => {
    try {
      const response = await GetUserCartItemsDetails(user?.userID!);
      if (response.isSuccess) {
        console.log(response.data);

        setCart(response.data!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  const loadProd = async () => {
    try {
      const response = await GetProducts();
      if (response.statusCode! > 200) {
        console.log(response.errorMessage);
      }
      if (response.isSuccess) {
        SetProducts(response.data!.slice(0, 3));
      }
    } catch (err) {
      console.error(err);
    }
  };
  const loadProdWithFilter = async () => {
    SetProducts(
      Products.filter(
        (product) =>
          !Cart.cartItems.find(
            (ct) => ct.product.productID === product.productID,
          ),
      ),
    );
  };

  useEffect(() => {
    if (Products.length !== 0 && Cart.cartItems.length > 0) {
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
