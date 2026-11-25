import { Link, useNavigate } from "react-router-dom";
import "../Shared/Shared.css";
import { useState } from "react";
import { InternoMicroserviceClient, LoginUserCommand } from "../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../../Core/Shared/getApiErrorMessage";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [submitted, setSubmitted] = useState(false);
  const [isPending, setIsPending] = useState(false);
  const [apiError, setApiError] = useState("");
  const [rememberMeChecked, setRememberMeChecked] = useState(false);
  const navigator = useNavigate();
  const { refreshUser } = useAuth();
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);

  const emailError =
    submitted && !EMAIL_PATTERN.test(email.trim())
      ? "Use a valid email format"
      : "";
  const passwordError =
    submitted && !password.trim() ? "Password Cant Be Empty" : "";

  function isValid() {
    return EMAIL_PATTERN.test(email.trim()) && password.trim().length > 0;
  }

  async function onLogin(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setSubmitted(true);
    setApiError("");
    if (!isValid()) return;

    setIsPending(true);
    try {
      const response = await Client.loginUser(
        new LoginUserCommand({
          email: email.trim(),
          password,
        }),
      );

      if (response.isSuccess) {
        localStorage.setItem("Token", response.data?.token!);
        localStorage.setItem("RefreshToken", response.data?.refreshToken!);
        localStorage.setItem("rememberMe", String(rememberMeChecked));
        refreshUser();
        navigator("/Home");
        return;
      }

      setApiError(
        getApiErrorMessage({
          statusCode: response.statusCode,
          errorMessage: response.errorMessage || "Invalid Data",
        }),
      );
    } catch (err) {
      setApiError(getApiErrorMessage(err));
    } finally {
      setIsPending(false);
    }
  }

  return (
    <section className="auth-section">
      <div className="auth-container">
        <div className="auth-image">
          <img
            src="https://images.unsplash.com/photo-1483985988355-763728e1935b?w=800&q=80"
            alt="Fashion"
          />
          <div className="image-overlay">
            <h2>Welcome Back</h2>
            <p>Login to access your account and continue shopping</p>
          </div>
        </div>

        <div className="auth-form-wrapper">
          <div className="auth-form-container">
            <div className="auth-header">
              <h1>Sign In</h1>
              <p>Enter your credentials to access your account</p>
            </div>

            <form className="auth-form" onSubmit={onLogin} noValidate>
              <div className="form-group">
                <label>Email Address</label>
                <input
                  type="email"
                  name="Email"
                  value={email}
                  onChange={(e) => setEmail(e.currentTarget.value)}
                  placeholder="you@example.com"
                />
                <span className="error">{emailError}</span>
              </div>

              <div className="form-group">
                <label>Password</label>
                <input
                  type="password"
                  name="Password"
                  value={password}
                  onChange={(e) => setPassword(e.currentTarget.value)}
                  placeholder="Enter your password"
                />
                <span className="error">{passwordError}</span>
              </div>

              <div className="form-options">
                <label className="checkbox-label">
                  <input
                    type="checkbox"
                    checked={rememberMeChecked}
                    onChange={(e) => setRememberMeChecked(e.currentTarget.checked)}
                  />
                  Remember me
                </label>
                <Link to="/Home/ForgotPassword" className="forgot-link">
                  Forgot password?
                </Link>
              </div>

              <button type="submit" className="btn-submit" disabled={isPending}>
                {isPending ? "Signing In..." : "Sign In"}
              </button>
              {apiError && <span className="error">{apiError}</span>}

              <div className="divider">
                <span>or continue with</span>
              </div>

              <div className="social-login">
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
              </div>

              <div className="auth-footer">
                Don't have an account?{" "}
                <Link to={"/Home/Register"}>Create one</Link>
              </div>
            </form>
          </div>
        </div>
      </div>
    </section>
  );
}
