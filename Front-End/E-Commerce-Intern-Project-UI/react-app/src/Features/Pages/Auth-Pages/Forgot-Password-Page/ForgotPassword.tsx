import { Link } from "react-router-dom";
import { useState } from "react";
import "../Shared/Shared.css";
import { requestPasswordReset } from "../../../../Core/Shared/AuthApiStubs";
import { getApiErrorMessage } from "../../../../Core/Shared/getApiErrorMessage";

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export default function ForgotPasswordPage() {
  const [email, setEmail] = useState("");
  const [submitted, setSubmitted] = useState(false);
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setSubmitted(true);
    setSuccess("");
    setError("");

    if (!EMAIL_PATTERN.test(email.trim())) {
      return;
    }

    setIsPending(true);
    try {
      await requestPasswordReset(email.trim());
      setSuccess("If an account exists for that email, a reset link will be sent.");
    } catch (err) {
      setError(getApiErrorMessage(err));
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
            <h2>Reset Access</h2>
            <p>We will help you get back into your Interno account</p>
          </div>
        </div>

        <div className="auth-form-wrapper">
          <div className="auth-form-container">
            <div className="auth-header">
              <h1>Forgot Password</h1>
              <p>Enter the email associated with your account</p>
            </div>

            <form className="auth-form" onSubmit={onSubmit} noValidate>
              <div className="form-group">
                <label>Email Address</label>
                <input
                  type="email"
                  name="Email"
                  value={email}
                  onChange={(e) => setEmail(e.currentTarget.value)}
                  placeholder="you@example.com"
                />
                <span className="error">
                  {submitted && !EMAIL_PATTERN.test(email.trim())
                    ? "Use a valid email format"
                    : ""}
                </span>
              </div>

              {error && <span className="error">{error}</span>}
              {success && <span className="form-hint">{success}</span>}

              <button type="submit" className="btn-submit" disabled={isPending}>
                {isPending ? "Sending..." : "Send Reset Link"}
              </button>

              <div className="auth-footer">
                Remembered it? <Link to="/Home/Login">Sign in</Link>
              </div>
            </form>
          </div>
        </div>
      </div>
    </section>
  );
}
