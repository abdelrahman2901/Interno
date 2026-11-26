import { Link } from "react-router-dom";
import { Package, RefreshCcw, Send, ClipboardList } from "lucide-react";
import PageLayout, { PageCrumb } from "./PageLayout";

type ReturnStep = {
  title: string;
  body: string;
  icon: "request" | "pack" | "ship" | "refund";
};

type PolicyBlock = {
  title: string;
  body: string;
};

const STEPS: ReturnStep[] = [
  {
    icon: "request",
    title: "Request",
    body: "Start a return from your order, or send us the order number if you checked out as a guest.",
  },
  {
    icon: "pack",
    title: "Pack",
    body: "Use the original packaging where you can. Include unworn items with tags still attached.",
  },
  {
    icon: "ship",
    title: "Ship",
    body: "Drop the prepaid label at any partner location. Keep the receipt until the refund is issued.",
  },
  {
    icon: "refund",
    title: "Refund",
    body: "Once we inspect the return, the refund goes back to the original payment method.",
  },
];

const POLICY: PolicyBlock[] = [
  {
    title: "Return window",
    body: "You have 30 days from delivery to request a return. Sale items follow the same window unless marked final sale.",
  },
  {
    title: "Item condition",
    body: "Pieces should be unworn, unwashed, and free of perfume or deodorant. Try them on over clean clothing.",
  },
  {
    title: "Non-returnable items",
    body: "Underwear, swimwear, earrings, gift cards, and items marked final sale cannot be returned for hygiene reasons.",
  },
  {
    title: "Refund timing",
    body: "Refunds are issued within 5 business days of arriving at the studio. Banks can take another 3-7 days to show the credit.",
  },
];

const ELIGIBLE = [
  "Unworn garments with original tags",
  "Faulty stitching or fabric flaws",
  "Wrong size or colour delivered",
  "Unused accessories in original box",
];

const NOT_ELIGIBLE = [
  "Items washed, altered, or worn outdoors",
  "Final-sale and sample-sale pieces",
  "Beauty, swim, and intimate apparel",
  "Returns requested after 30 days",
];

const ICONS = {
  request: ClipboardList,
  pack: Package,
  ship: Send,
  refund: RefreshCcw,
};

const CRUMBS: PageCrumb[] = [
  { label: "Home", to: "/Home" },
  { label: "Returns" },
];

export default function ReturnsPage() {
  return (
    <PageLayout
      title="Returns"
      subtitle="A 30-day window, prepaid labels, and refunds back to the original payment method."
      crumbs={CRUMBS}
    >
      <section className="info-section">
        <h2>How a return works</h2>
        <div className="info-grid cols-4">
          {STEPS.map((step) => {
            const Icon = ICONS[step.icon];
            return (
              <article className="info-card" key={step.title}>
                <span className="info-icon">
                  <Icon size={18} aria-hidden="true" />
                </span>
                <h3>{step.title}</h3>
                <p>{step.body}</p>
              </article>
            );
          })}
        </div>
      </section>

      <section className="info-section">
        <h2>Policy details</h2>
        <div className="info-grid cols-2">
          {POLICY.map((block) => (
            <article className="info-card" key={block.title}>
              <h3>{block.title}</h3>
              <p>{block.body}</p>
            </article>
          ))}
        </div>
      </section>

      <section className="info-section">
        <div className="info-split">
          <article className="info-card info-eligible">
            <h3>Eligible</h3>
            <ul className="info-list">
              {ELIGIBLE.map((item) => (
                <li key={item}>{item}</li>
              ))}
            </ul>
          </article>
          <article className="info-card info-ineligible">
            <h3>Not eligible</h3>
            <ul className="info-list">
              {NOT_ELIGIBLE.map((item) => (
                <li key={item}>{item}</li>
              ))}
            </ul>
          </article>
        </div>
      </section>

      <section className="info-card info-help-cta">
        <div>
          <h2>Ready to send something back?</h2>
          <p className="info-lead">
            Share your order number and we will email a prepaid label the same day.
          </p>
        </div>
        <Link to="/Home/Contact" className="info-btn">
          Start a return
        </Link>
      </section>
    </PageLayout>
  );
}
