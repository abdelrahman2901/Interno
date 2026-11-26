import { Link, useNavigate } from "react-router-dom";
import "../Shared/Shared.css";
import { useState } from "react";
import { Eye, EyeOff } from "lucide-react";
import { IError } from "../../../../Core/Interface/Error/IError";
import { InternoMicroserviceClient, RegisterUserCommand } from "../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../../Core/Shared/getApiErrorMessage";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export default function RegisterPage() {
  const [Errors, SetErrors] = useState<IError>();
  const { refreshUser } = useAuth();
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [request, setRegisterRequest] = useState<RegisterUserCommand>(
    new RegisterUserCommand(),
  );
  const navigator = useNavigate();
  const [isFormSubmitted, setIsFormSubmitted] = useState<boolean>(false);
  const [isPending, setIsPending] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  function formValidation() {
    const email = request.email ?? "";
    const phone = request.phoneNumber ?? "";
    const password = request.password ?? "";
    const confirmPassword = request.confirmPassword ?? "";
    if (!(request.userName ?? "").trim()) return false;
    if (!EMAIL_PATTERN.test(email.trim())) return false;
    if (phone.trim().length < 11) return false;
    if (password.length < 5) return false;
    if (!confirmPassword.trim()) return false;
    if (password !== confirmPassword) return false;
    return true;
  }
  async function onRegister(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setIsFormSubmitted(true);
    SetErrors(undefined);

    if (!formValidation()) {
      if (
        request.password &&
        request.confirmPassword &&
        request.password !== request.confirmPassword
      ) {
        SetErrors({
          errorType: "ConfirmPassword",
          ErrorMessage: "Password Doesnt Match",
        });
      }
      return;
    }

    setIsPending(true);
    try {
      const registerResponse = await Client.registerUser(request);
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
          errorType: "Invalid Data",
          ErrorMessage: getApiErrorMessage({
            statusCode: registerResponse.statusCode,
            errorMessage: registerResponse.errorMessage || "Invalid Data",
          }),
        });
      }
    } catch (err) {
      SetErrors({
        errorType: "Invalid Data",
        ErrorMessage: getApiErrorMessage(err),
      });
    } finally {
      setIsPending(false);
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

              <form className="auth-form" onSubmit={onRegister} noValidate>
                <div className="form-group">
                  <label>User Name</label>
                  <input
                    type="text"
                    name="UserName"
                    onChange={(e) => {
                      const username = e.currentTarget.value;
                      setRegisterRequest(
                        (prev) =>
                          new RegisterUserCommand({
                            ...prev,
                            userName: username,
                          }),
                      );
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
                      setRegisterRequest(
                        (prev) =>
                          new RegisterUserCommand({
                            ...prev,
                            email: email,
                          }),
                      );
                    }}
                    placeholder="you@example.com"
                  />
                  <span className="error">
                    {isFormSubmitted &&
                    !EMAIL_PATTERN.test((request.email ?? "").trim())
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
                      setRegisterRequest(
                        (prev) =>
                          new RegisterUserCommand({
                            ...prev,
                            phoneNumber: phoneNumber,
                          }),
                      );
                    }}
                    placeholder="Minimum 11 characters"
                  />
                  <span className="error">
                    {!request.phoneNumber && isFormSubmitted
                      ? "PhoneNumber Cant Be Empty"
                      : ""}
                  </span>
                  <span className="error">
                    {(request.phoneNumber?.length ?? 0) < 11 && isFormSubmitted
                      ? "PhoneNumber Must Has More Than 11 digits"
                      : ""}
                  </span>
                </div>

                <div className="form-group">
                  <label>Password</label>
                  <div className="password-field">
                    <input
                      type={showPassword ? "text" : "password"}
                      name="Password"
                      onChange={(e) => {
                        const password = e.currentTarget.value;
                        setRegisterRequest(
                          (prev) =>
                            new RegisterUserCommand({
                              ...prev,
                              password: password,
                            }),
                        );
                      }}
                      placeholder="Minimum 5 characters"
                    />
                    <button
                      type="button"
                      className="password-toggle"
                      onClick={() => setShowPassword((open) => !open)}
                      aria-label={showPassword ? "Hide password" : "Show password"}
                    >
                      {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                    </button>
                  </div>
                  <small className="form-hint">
                    Must be at least 5 characters long
                  </small>
                  <span className="error">
                    {Errors?.errorType === "Password"
                      ? Errors.ErrorMessage
                      : ""}
                  </span>
                  <span className="error">
                    {(request.password?.length ?? 0) < 5 && isFormSubmitted
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
                      setRegisterRequest(
                        (prev) =>
                          new RegisterUserCommand({
                            ...prev,
                            confirmPassword: confirmPassword,
                          }),
                      );
                    }}
                    placeholder="Re-enter password"
                  />
                  <span className="error">
                    {Errors?.errorType === "ConfirmPassword"
                      ? Errors.ErrorMessage
                      : ""}
                  </span>
                  <span className="error">
                    {(request.confirmPassword?.length ?? 0) < 5 && isFormSubmitted
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

                {Errors?.errorType === "Invalid Data" && (
                  <span className="error">{Errors.ErrorMessage}</span>
                )}
                <button
                  type="submit"
                  className="btn-submit"
                  disabled={isPending}
                >
                  {isPending ? "Creating Account..." : "Create Account"}
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
