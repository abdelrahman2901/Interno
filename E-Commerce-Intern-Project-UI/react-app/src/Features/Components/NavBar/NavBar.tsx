import { Link, useNavigate } from "react-router-dom";
import "./NavBar.css";
import { useEffect, useState } from "react";
import {
  Bell,
  Heart,
  ShoppingCart,
  Menu,
  ChevronRight,
  Search,
  X,
} from "lucide-react";
import { CartDetails } from "../../../Core/DTO/CartDTO/CartDetails.js";
import { NotificationModel } from "../../../Core/DTO/NotificationDTO/NotificationModel.js";
import { useCartStore } from "../../../Core/Global-State-Management/Zusstand/CartStore/CartUpdateStore.js";
import { ISubCategoryDetails } from "../../../Core/Interface/Category/ISubCateogryDetails.js";
import { useAuth } from "../../../Core/Services/AuthServices/AuthProvider.js";
import { SignOut } from "../../../Core/Services/AuthServices/AuthUserService.js";
import { GetUserCartItemsDetails } from "../../../Core/Services/CartServices/CartService.js";
import { GetSubCategoryDetails } from "../../../Core/Services/CategoryServices/CategoryServiceQuery.js";
import {
  GetAllUserNotifications,
  MarkNotificationAsRead,
  MarkNotificationAsReadList,
} from "../../../Core/Services/NotificationServices/NotificationService.js";
import { useExclusiveDropdowns } from "../../../Core/Shared/hooks/useDropdown.js";
import CategoryDropdown from "./CategoryDropdown.js";
import UserMenu from "./UserMenu.js";

export default function Navbar() {
  const [mobileDropDown, setMobileDropDown] = useState<boolean>(false);
  const { user, signout } = useAuth();
  const [categories, setCategories] = useState<ISubCategoryDetails[]>([]);
  const [_notifications, setPrivNotifications] = useState<NotificationModel[]>(
    [],
  );
  const [notifications, setNotifications] = useState<NotificationModel[]>([]);
  const [unreadCount, setUnreadCount] = useState<number>(0);
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const [cartItems, setCartItems] = useState<CartDetails>(new CartDetails());
  const [searchOpen, setSearchOpen] = useState<boolean>(false);
  const [searchQuery, setSearchQuery] = useState<string>("");
  const navigator = useNavigate();
  const { cartVersion } = useCartStore();
  const { toggleDropdown, isDropdownOpen } = useExclusiveDropdowns([
    "categories",
    "account",
  ]);

  const loadCartItems = async () => {
    if (!user?.userID) return;

    try {
      const response = await GetUserCartItemsDetails(user.userID);
      if (response.isSuccess && response.data) {
        setCartItems(response.data);
      }
    } catch (err) {}
  };
  const loadCats = async () => {
    try {
      const response = await GetSubCategoryDetails();
      if (response.isSuccess && response.data) {
        setCategories(response.data);
      }
    } catch (err) {}
  };
  function onSignOut() {
    const SignOutreq = async () => {
      try {
        const response = await SignOut(user?.userID!);
        if (response.isSuccess) {
          signout();
          navigator("/Home");
        }
      } catch (err) {}
    };
    SignOutreq();
  }
  function ExtractDate(CreateDate: string) {
    const date = new Date(CreateDate);
    return `${date.getDate()}-${date.getMonth()}-${date.getFullYear()}`;
  }
  const loadNotifciation = async () => {
    try {
      const response = await GetAllUserNotifications(user?.userID!);
      if (response.data && response.isSuccess) {
        setUnreadCount(response.data.slice().filter((r) => !r.isRead).length);
        setPrivNotifications(response.data);
        setNotifications(response.data.slice(0, 4));
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    loadCartItems();
  }, [cartVersion]);
  useEffect(() => {
    if (categories.length === 0) {
      loadCats();
    }
  }, []);

  useEffect(() => {
    if (user) {
      loadCartItems();
    } else {
      setCartItems(new CartDetails());
    }
    if (notifications.length === 0 && user) {
      loadNotifciation();
    }
  }, [user]);

  function onToggle() {
    setIsOpen(!isOpen);
  }

  function toggleMobileMenu() {
    setMobileDropDown(!mobileDropDown);
  }

  function closeMobileMenu() {
    setMobileDropDown(false);
  }

  function toggleSearch() {
    setSearchOpen(!searchOpen);
    if (!searchOpen) {
      setSearchQuery("");
    }
  }

  function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    if (searchQuery.trim()) {
      navigator(
        `/Home/Products?search=${encodeURIComponent(searchQuery.trim())}`,
      );
      setSearchOpen(false);
      setSearchQuery("");
    }
  }

  function closeSearch() {
    setSearchOpen(false);
    setSearchQuery("");
  }
  async function handleNotificationClick(notificationID: string) {
    try {
      const response = await MarkNotificationAsRead(notificationID);
      if (response.data && response.isSuccess) {
        loadNotifciation();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }
  async function onMarkAllAsRead() {
    try {
      const response = await MarkNotificationAsReadList(user?.userID!);
      if (response.data && response.isSuccess) {
        loadNotifciation();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }

  return (
    <nav className="navbar">
      <div className="nav-container">
        <button
          className="hamburger"
          onClick={toggleMobileMenu}
          aria-label="Toggle mobile menu"
          aria-expanded={mobileDropDown}
        >
          <Menu size={22} />
        </button>

        <div className="logo">
          <div className="logo-text">
            Interno
            <span className="logo-sub">CLOTHING BRAND</span>
          </div>
        </div>

        <div
          className="nav-links"
          id="navLinks"
          role="navigation"
          aria-label="Main navigation"
        >
          <Link to={"/Home"} className="nav-link" tabIndex={0}>
            Home
          </Link>

          <CategoryDropdown
            categories={categories}
            isDropdownOpen={isDropdownOpen("categories")}
            onToggleDropdown={() => toggleDropdown("categories")}
          />

          <Link to={"/Home/About"} className="nav-link" tabIndex={0}>
            About
          </Link>

          <Link to={"/Home/Contact"} className="nav-link" tabIndex={0}>
            Contact
          </Link>

          <Link
            to={"/Home/Sale"}
            className="nav-link nav-sale-link"
            tabIndex={0}
          >
            Sale
          </Link>
        </div>

        <div className="nav-icons">
          <div className="notification-dropdown-container">
            <div className="notification-trigger" onClick={onToggle}>
              <span className="icon-btn" title="Notifications">
                <Bell size={18} />
              </span>
              {unreadCount > 0 && (
                <span className="notification-badge">{unreadCount}</span>
              )}
            </div>

            {isOpen && (
              <div className="notification-dropdown">
                <div className="notification-header">
                  <h3>Notifications</h3>
                  {notifications.length > 0 && (
                    <div className="notification-actions">
                      {unreadCount > 0 && (
                        <button
                          className="action-btn"
                          onClick={onMarkAllAsRead}
                        >
                          Mark all read
                        </button>
                      )}
                    </div>
                  )}
                </div>

                <div className="notification-list">
                  {notifications.length === 0 ? (
                    <div className="notification-empty">
                      <div className="empty-icon">
                        <Bell size={48} opacity={0.5} />
                      </div>
                      <p>No notifications</p>
                      <small>You're all caught up!</small>
                    </div>
                  ) : (
                    notifications.map((notification) => (
                      <div
                        key={notification.notificationID}
                        className={`notification-item ${!notification.isRead ? "unread" : ""}`}
                        onClick={() =>
                          handleNotificationClick(notification.notificationID)
                        }
                      >
                        {!notification.isRead && (
                          <span className="unread-dot"></span>
                        )}

                        <div className="notification-icon">✅</div>

                        <div className="notification-content">
                          <div className="notification-message">
                            {notification.message}
                          </div>
                          <div className="notification-time">
                            {ExtractDate(notification.createdAt)}
                            {/* {getTimeAgo(notification.timestamp)} */}
                          </div>
                        </div>
                      </div>
                    ))
                  )}
                </div>

                {_notifications.length > 5 && (
                  <div className="notification-footer">
                    <button
                      type="button"
                      className="view-all-link"
                      onClick={() => {
                        setNotifications(_notifications);
                      }}
                    >
                      View All Notifications
                    </button>
                  </div>
                )}
              </div>
            )}
          </div>
          <span className="icon-btn" title="Wishlist">
            <Link to={"/Home/WishList"} className="nav-link">
              <Heart size={18} />
            </Link>
          </span>
          <div className="cart-icon">
            <span className="icon-btn">
              <Link to={"/Home/Cart"} className="nav-link">
                <ShoppingCart size={18} />
              </Link>
            </span>
            <span className="cart-badge ">{cartItems.cartItems.length}</span>
          </div>
          {!user && (
            <>
              <Link to="/Home/Login" className="auth-btn login-btn">
                Login
              </Link>
              <Link to="/Home/Register" className="auth-btn register-btn">
                Register
              </Link>
            </>
          )}

          {user && (
            <UserMenu
              user={user}
              onSignOut={onSignOut}
              isDropdownOpen={isDropdownOpen("account")}
              onToggleDropdown={() => toggleDropdown("account")}
            />
          )}
        </div>
      </div>

      <div className={"mobile-menu " + (mobileDropDown ? "active" : "")}>
        <Link to="/Home" className="mobile-item" onClick={closeMobileMenu}>
          Home
        </Link>
        <div className="mobile-item">Categories</div>
       
        <Link
          to="/Home/About"
          className="mobile-item"
          onClick={closeMobileMenu}
        >
          About
        </Link>
        <Link
          to="/Home/Contact"
          className="mobile-item"
          onClick={closeMobileMenu}
        >
          Contact
        </Link>
        <Link
          to="/Home/Sale"
          className="mobile-item sale-text"
          onClick={closeMobileMenu}
        >
          Sale
        </Link>
      </div>
    </nav>
  );
}
