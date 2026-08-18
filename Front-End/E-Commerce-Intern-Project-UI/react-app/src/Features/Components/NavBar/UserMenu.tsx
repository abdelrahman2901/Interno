import { Link } from 'react-router-dom';
import { getAvatarUrl } from '../../../Core/config';
import { IUser } from '../../../Core/Interface/User-Interfaces/IUser';

interface UserMenuProps {
  user: IUser;
  onSignOut: () => void;
  isDropdownOpen: boolean;
  onToggleDropdown: () => void;
}

export default function UserMenu({ user, onSignOut, isDropdownOpen, onToggleDropdown }: UserMenuProps) {
  return (
    <div className="dropdown">
      <div className="user-profile">
        <img
          src={getAvatarUrl(user.personName)}
          alt="User Avatar"
        />
        <span
          className="nav-link dropdown-trigger"
          onClick={onToggleDropdown}
        >
          {user.personName} <span className="arrow">▾</span>
        </span>
      </div>

      <div className={`dropdown-menu categories-menu ${isDropdownOpen ? "show" : ""}`}>
        <div className="dropdown-item">
          <Link to={"/Home/ManageAccount"} className="nav-link">
            <span className="icon">✦</span> Manage Account
          </Link>
        </div>
        {user.role === "Admin" && (
          <div className="dropdown-item">
            <Link to={"/Home/Admin-DashBoard"} className="nav-link">
              <span className="icon">✦</span> Admin Dashboard
            </Link>
          </div>
        )}
        <div className="dropdown-item">
          <button
            className="auth-btn register-btn"
            onClick={onSignOut}
          >
            Sign Out
          </button>
        </div>
      </div>
    </div>
  );
}
