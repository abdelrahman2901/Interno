import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import Navbar from "./Features/Components/NavBar/NavBar";
import Admin_Dashboard from "./Features/Pages/Admin-Dashboard/Admin-DashBoard-Page";
import LoginPage from "./Features/Pages/Auth-Pages/Login-Page/Login";
import RegisterPage from "./Features/Pages/Auth-Pages/Register-Page/Register";
import Home from "./Features/Pages/Home-Page/Home";
import ManageAccount from "./Features/Pages/Manage-Account/ManageAccount";
import WishListPage from "./Features/Pages/WishList-Page/WIshListPage";
import CartPage from "./Features/Pages/Cart-Page/CartPage";
import PaymentPage from "./Features/Pages/Payment-Page/PaymentPage";
import "./App.css";
import ProductsPage from "./Features/Pages/Products-Page/ProductsPage";
import AdminAuthGuard from "./Core/Guards/AdminAuthGuard";
import UnAuthGuard from "./Core/Guards/UnAuthGuard";
export default function App() {
  return (
    <>
      <Router>
        <Navbar />
        <div className="bg-root">
          <Routes>
            <Route path="/" Component={Home}></Route>
            <Route path="/Home" Component={Home}></Route>
            <Route path="/Home/Login" Component={LoginPage}></Route>
            <Route path="/Home/Register" Component={RegisterPage}></Route>
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
      </Router>
    </>
  );
}
