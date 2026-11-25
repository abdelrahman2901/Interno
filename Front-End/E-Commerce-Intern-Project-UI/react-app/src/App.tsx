import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import Navbar from "./Features/Components/NavBar/NavBar";
import SmoothScroll from "./Features/Components/Shared/SmoothScroll";
import Admin_Dashboard from "./Features/Pages/Admin-Dashboard/Admin-DashBoard-Page";
import LoginPage from "./Features/Pages/Auth-Pages/Login-Page/Login";
import RegisterPage from "./Features/Pages/Auth-Pages/Register-Page/Register";
import ForgotPasswordPage from "./Features/Pages/Auth-Pages/Forgot-Password-Page/ForgotPassword";
import Home from "./Features/Pages/Home-Page/Home";
import ManageAccount from "./Features/Pages/Manage-Account/ManageAccount";
import WishListPage from "./Features/Pages/WishList-Page/WIshListPage";
import CartPage from "./Features/Pages/Cart-Page/CartPage";
import PaymentPage from "./Features/Pages/Payment-Page/PaymentPage";
import "./App.css";
import ProductsPage from "./Features/Pages/Products-Page/ProductsPage";
import AboutPage from "./Features/Pages/Static-Pages/AboutPage";
import ContactPage from "./Features/Pages/Static-Pages/ContactPage";
import FaqPage from "./Features/Pages/Static-Pages/FaqPage";
import ShippingPage from "./Features/Pages/Static-Pages/ShippingPage";
import ReturnsPage from "./Features/Pages/Static-Pages/ReturnsPage";
import SizeGuidePage from "./Features/Pages/Static-Pages/SizeGuidePage";
import AdminAuthGuard from "./Core/Guards/AdminAuthGuard";
import UnAuthGuard from "./Core/Guards/UnAuthGuard";

export default function App() {
  return (
    <>
      <Router>
        <SmoothScroll>
          <Navbar />
          <div className="bg-root">
            <Routes>
              <Route path="/" Component={Home}></Route>
              <Route path="/Home" Component={Home}></Route>
              <Route path="/Home/Login" Component={LoginPage}></Route>
              <Route path="/Home/Register" Component={RegisterPage}></Route>
              <Route
                path="/Home/ForgotPassword"
                Component={ForgotPasswordPage}
              ></Route>
              <Route path="/Home/About" Component={AboutPage}></Route>
              <Route path="/Home/Contact" Component={ContactPage}></Route>
              <Route path="/Home/FAQ" Component={FaqPage}></Route>
              <Route path="/Home/Shipping" Component={ShippingPage}></Route>
              <Route path="/Home/Returns" Component={ReturnsPage}></Route>
              <Route path="/Home/SizeGuide" Component={SizeGuidePage}></Route>
              <Route
                path="/Home/Admin-DashBoard"
                element={
                  <AdminAuthGuard>
                    <Admin_Dashboard />
                  </AdminAuthGuard>
                }
              ></Route>
              <Route
                path="/Home/ManageAccount"
                element={
                  <UnAuthGuard>
                    <ManageAccount />
                  </UnAuthGuard>
                }
              ></Route>
              <Route path="/Home/WishList" Component={WishListPage}></Route>
              <Route path="/Home/Cart" Component={CartPage}></Route>
              <Route path="/Home/Products" Component={ProductsPage}></Route>
              <Route
                path="/Home/Cart/OrderPayment"
                element={
                  <UnAuthGuard>
                    <PaymentPage />
                  </UnAuthGuard>
                }
              ></Route>
            </Routes>
          </div>
        </SmoothScroll>
      </Router>
    </>
  );
}
