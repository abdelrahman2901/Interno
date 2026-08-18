import { Link, Navigate, useNavigate } from "react-router-dom";
import "../Shared/Shared.css";
import React, { useEffect, useState } from "react";
import { LoginRequest } from "../../../../Core/DTO/AuthDTO/LoginRequest";
import { IError } from "../../../../Core/Interface/Error/IError";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
import { Login } from "../../../../Core/Services/AuthServices/AuthUserService";

export default function LoginPage() {
  const [Errors, SetErrors] = useState<IError>();
  const [remeberMeChecked, setRemeberMeChecked] = useState<boolean>(false);
  const navigator = useNavigate();
  const { refreshUser } = useAuth();
  function onLogin(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const form = new FormData(e.currentTarget);
    const request: LoginRequest = {
      email: form.get("Email") as string,
      password: form.get("Password") as string,
    };

    const loginRequest = async () => {
      try {
        const response = await Login(request);
        console.log(response);

        if (response.isSuccess) {
          localStorage.setItem("Token", response.data?.token!);
          localStorage.setItem("RefreshToken", response.data?.refreshToken!);
          localStorage.setItem("rememberMe", String(remeberMeChecked));
          refreshUser();
          navigator("/Home");
          SetErrors({ errorType: "", ErrorMessage: "" });
        } else {
          SetErrors({
            errorType: response.statusCode === 404 ? "User" : "Invalid Data",
            ErrorMessage: response.errorMessage!,
          });
        }
      } catch (err) {
        if (err) {
          console.error(err);
          SetErrors({
            errorType: "429",
            ErrorMessage: "Too many Requests , Please Try Again Later",
          });
          console.log(Errors);
        }
      }
    };
    loginRequest();
  }
  return (
    <>
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

              <form className="auth-form" onSubmit={onLogin}>
                <div className="form-group">
                  <label>Email Address</label>
                  <input
                    type="email"
                    name="Email"
                    required
                    placeholder="you@example.com"
                  />
                </div>

                <div className="form-group">
                  <label>Password</label>
                  <input
                    type="password"
                    name="Password"
                    required
                    placeholder="Enter your password"
                  />
                </div>

                <div className="form-options">
                  <label className="checkbox-label">
                    <input
                      type="checkbox"
                      onChange={(e) => {
                        e.preventDefault();
                        setRemeberMeChecked(e.currentTarget.checked);
                      }}
                    />
                    <span>Remember me</span>
                  </label>
                  <a href="#" className="forgot-link">
                    Forgot password?
                  </a>
                </div>

                <button type="submit" className="btn-submit">
                  Sign In
                </button>
                {Errors?.errorType === "User" && (
                  <span className="error">Email Was Not Found</span>
                )}
                {Errors?.errorType === "Invalid Data" && (
                  <span className="error">{Errors.ErrorMessage}</span>
                )}
                {Errors?.errorType === "429" && (
                  <span className="error">{Errors.ErrorMessage}</span>
                )}

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
    </>
  );
}
