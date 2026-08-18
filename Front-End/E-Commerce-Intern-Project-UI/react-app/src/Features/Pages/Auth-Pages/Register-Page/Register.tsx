import { Link, useNavigate } from "react-router-dom";
import "../Shared/Shared.css";
import { useState } from "react";
import { IError } from "../../../../Core/Interface/Error/IError";
import { Register } from "../../../../Core/Services/AuthServices/AuthUserService";
import { RegisterRequest } from "../../../../Core/DTO/AuthDTO/RegisterRequest";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";

export default function RegisterPage() {
  const [Errors, SetErrors] = useState<IError>();
  const { refreshUser } = useAuth();
  const [request, setRegisterRequest] = useState<RegisterRequest>(
    new RegisterRequest(),
  );
  const navigator = useNavigate();
  const [isFormSubmitted, setIsFormSubmitted] = useState<boolean>(false);
  function formValidation() {
    if (
      request.userName === "" ||
      request.phoneNumber === "" ||
      request.email === "" ||
      request.password === "" ||
      request.confirmPassword === ""
    ) {
      return false;
    }
    return true;
  }
  async function onRegister(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setIsFormSubmitted(true);
    console.log(request);

    if (!formValidation()) {
      return;
    }
    if (request.password !== request.confirmPassword) {
      SetErrors({
        errorType: "ConfirmPassword",
        ErrorMessage: "Password Doesnt Match",
      });
      return;
    }

    try {
      const registerResponse = await Register(request);
      console.log(registerResponse);
      if (registerResponse.isSuccess) {
        localStorage.setItem("Token", registerResponse.data?.token!);
        localStorage.setItem(
          "RefreshToken",
          registerResponse.data?.refreshToken!,
        );

        refreshUser();

        navigator("/Home");
      } else {
        SetErrors({
          errorType:
            registerResponse.statusCode === 404 ? "User" : "Invalid Data",
          ErrorMessage: registerResponse.errorMessage!,
        });
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }
  return (
    <>
      <section className="auth-section">
        <div className="auth-container">
          <div className="auth-image">
            <img
              src="https://images.unsplash.com/photo-1490578474895-699cd4e2cf59?w=800&q=80"
              alt="Fashion"
            />
            <div className="image-overlay">
              <h2>Join Interno</h2>
              <p>Create an account and enjoy exclusive benefits</p>
              <ul className="benefits-list">
                <li>✓ Exclusive member discounts</li>
                <li>✓ Early access to new collections</li>
                <li>✓ Free shipping on first order</li>
                <li>✓ Birthday rewards</li>
              </ul>
            </div>
          </div>

          <div className="auth-form-wrapper">
            <div className="auth-form-container">
              <div className="auth-header">
                <h1>Create Account</h1>
                <p>Join us and start your shopping journey</p>
              </div>

              <form className="auth-form" onSubmit={onRegister}>
                <div className="form-group">
                  <label>User Name</label>
                  <input
                    type="text"
                    name="UserName"
                    onChange={(e) => {
                      const username = e.currentTarget.value;
                      setRegisterRequest((prev) => ({
                        ...prev,
                        userName: username,
                      }));
                    }}
                    placeholder="John Alex"
                  />

                  <span className="error">
                    {!request.userName && isFormSubmitted
                      ? "User Name Cant Be Empty"
                      : ""}
                  </span>
                </div>

                <div className="form-group">
                  <label>Email Address</label>
                  <input
                    type="email"
                    name="Email"
                    onChange={(e) => {
                      const email = e.currentTarget.value;
                      setRegisterRequest((prev) => ({
                        ...prev,
                        email: email,
                      }));
                    }}
                    placeholder="you@example.com"
                  />
                  <span className="error">
                    {!request.email.includes("@") ||
                    (!request.email.includes(".com") && isFormSubmitted)
                      ? "Use Valid Email Format"
                      : ""}
                  </span>
                  <span className="error">
                    {Errors?.errorType === "Email" ? Errors.ErrorMessage : ""}
                  </span>
                  <br />
                  <span className="error">
                    {!request.email && isFormSubmitted
                      ? "Email Cant Be Empty"
                      : ""}
                  </span>
                </div>

                <div className="form-group">
                  <label>Phone Number</label>
                  <input
                    type="text"
                    name="PhoneNumber"
                    onChange={(e) => {
                      const phoneNumber = e.currentTarget.value;
                      setRegisterRequest((prev) => ({
                        ...prev,
                        phoneNumber: phoneNumber,
                      }));
                    }}
                    placeholder="Minimum 11 characters"
                  />
                  <span className="error">
                    {!request.phoneNumber && isFormSubmitted
                      ? "PhoneNumber Cant Be Empty"
                      : ""}
                  </span>
                  <span className="error">
                    {request.phoneNumber.length < 11 && isFormSubmitted
                      ? "PhoneNumber Must Has More Than 11 digits"
                      : ""}
                  </span>
                </div>

                <div className="form-group">
                  <label>Password</label>
                  <input
                    type="password"
                    name="Password"
                    onChange={(e) => {
                      const password = e.currentTarget.value;
                      setRegisterRequest((prev) => ({
                        ...prev,
                        password: password,
                      }));
                    }}
                    placeholder="Minimum 5 characters"
                  />
                  <small className="form-hint">
                    Must be at least 5 characters long
                  </small>
                  <span className="error">
                    {Errors?.errorType === "Password"
                      ? Errors.ErrorMessage
                      : ""}
                  </span>
                  <span className="error">
                    {request.password.length < 5 && isFormSubmitted
                      ? "password Must Has More Than 5 digits"
                      : ""}
                  </span>
                </div>

                <div className="form-group">
                  <label>Confirm Password</label>
                  <input
                    type="password"
                    name="ConfirmPassword"
                    onChange={(e) => {
                      const confirmPassword = e.currentTarget.value;
                      setRegisterRequest((prev) => ({
                        ...prev,
                        confirmPassword: confirmPassword,
                      }));
                    }}
                    placeholder="Re-enter password"
                  />
                  <span className="error">
                    {Errors?.errorType === "ConfirmPassword"
                      ? Errors.ErrorMessage
                      : ""}
                  </span>
                  <span className="error">
                    {request.confirmPassword.length < 5 && isFormSubmitted
                      ? "password Must Has More Than 8 digits"
                      : ""}
                  </span>
                  <span className="error">
                    {request.confirmPassword === "" && isFormSubmitted
                      ? "Confirm Password Cant Empty"
                      : ""}
                  </span>
                </div>

                {/* <div className="form-group">
                  <label className="checkbox-label">
                    <input type="checkbox" name="newsletter" />
                    <span>Subscribe to newsletter for exclusive offers</span>
                  </label>
                </div>

                <div className="form-group">
                  <label className="checkbox-label">
                    <input type="checkbox" name="terms " />
                    <span>
                      I agree to the{" "}
                      <a href="#" className="link">
                        Terms & Conditions
                      </a>{" "}
                      and{" "}
                      <a href="#" className="link">
                        Privacy Policy
                      </a>
                    </span>
                  </label>
                </div> */}

                <button type="submit" className="btn-submit">
                  Create Account
                </button>

                {/* <div className="divider">
                  <span>or sign up with</span>
                </div> */}

                {/* <div className="social-login">
                  <button type="button" className="social-btn google">
                    <svg width="18" height="18" viewBox="0 0 18 18">
                      <path
                        fill="#4285F4"
                        d="M17.64 9.2c0-.637-.057-1.251-.164-1.84H9v3.481h4.844c-.209 1.125-.843 2.078-1.796 2.717v2.258h2.908c1.702-1.567 2.684-3.874 2.684-6.615z"
                      />
                      <path
                        fill="#34A853"
                        d="M9.003 18c2.43 0 4.467-.806 5.956-2.184l-2.908-2.258c-.806.54-1.837.86-3.048.86-2.344 0-4.328-1.584-5.036-3.711H.96v2.332C2.44 15.983 5.485 18 9.003 18z"
                      />
                      <path
                        fill="#FBBC05"
                        d="M3.964 10.707c-.18-.54-.282-1.117-.282-1.707 0-.593.102-1.17.282-1.709V4.958H.957C.347 6.173 0 7.548 0 9c0 1.452.348 2.827.957 4.042l3.007-2.335z"
                      />
                      <path
                        fill="#EA4335"
                        d="M9.003 3.58c1.321 0 2.508.454 3.44 1.345l2.582-2.58C13.464.891 11.426 0 9.003 0 5.485 0 2.44 2.017.96 4.958L3.967 7.29c.708-2.127 2.692-3.71 5.036-3.71z"
                      />
                    </svg>
                    Google
                  </button>
                  <button type="button" className="social-btn facebook">
                    <svg
                      width="18"
                      height="18"
                      viewBox="0 0 24 24"
                      fill="#1877F2"
                    >
                      <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z" />
                    </svg>
                    Facebook
                  </button>
                </div> */}

                <div className="auth-footer">
                  Already have an account? <Link to="/Home/Login">Sign in</Link>
                </div>
              </form>
            </div>
          </div>
        </div>
      </section>
    </>
  );
}
