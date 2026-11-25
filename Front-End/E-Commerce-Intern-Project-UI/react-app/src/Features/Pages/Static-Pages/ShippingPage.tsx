import { Clock, Globe, MapPin } from "lucide-react";
import PageLayout, { PageCrumb } from "./PageLayout";

type ShippingMethod = {
  name: string;
  price: string;
  delivery: string;
  threshold: string;
};

type InfoCard = {
  title: string;
  body: string;
  icon: "clock" | "pin" | "globe";
};

type TimelineStep = {
  title: string;
  body: string;
};

const METHODS: ShippingMethod[] = [
  {
    name: "Standard",
    price: "$6.50",
    delivery: "5-7 business days",
    threshold: "Free over $75",
  },
  {
    name: "Express",
    price: "$14.90",
    delivery: "2-3 business days",
    threshold: "Free over $150",
  },
  {
    name: "Next-day",
    price: "$24.90",
    delivery: "Next business day if ordered by 14:00",
    threshold: "Not eligible for free shipping",
  },
];

const INFO_CARDS: InfoCard[] = [
  {
    icon: "clock",
    title: "Processing time",
    body: "In-stock pieces leave our studio in 1 business day. Made-to-order knits take 3-5 days before they are handed to the carrier.",
  },
  {
    icon: "pin",
    title: "Tracking",
    body: "A tracking link is emailed the moment a label is created. You can also follow the parcel from Manage Account.",
  },
  {
    icon: "globe",
    title: "International shipping",
    body: "We ship to 28 countries. Duties are prepaid at checkout. Delivery windows are 6-12 business days depending on customs.",
  },
];

const TIMELINE: TimelineStep[] = [
  {
    title: "We confirm the order",
    body: "Payment clears and you receive an email with the packing list.",
  },
  {
    title: "The studio prepares it",
    body: "Items are picked, quality checked, and packed with recycled tissue.",
  },
  {
    title: "The carrier collects",
    body: "A tracking number goes live and the parcel joins the evening dispatch.",
  },
  {
    title: "It arrives with you",
    body: "Most local orders are delivered to the door. Signature is required over $200.",
  },
];

const NOTES = [
  "Weather, peak sale periods, and public holidays can add 1-2 days.",
  "Remote postcodes may only be served by Standard shipping.",
  "Address changes are free until the label is printed.",
  "PO boxes are accepted for Standard deliveries only.",
];

const ICONS = {
  clock: Clock,
  pin: MapPin,
  globe: Globe,
};

const CRUMBS: PageCrumb[] = [
  { label: "Home", to: "/Home" },
  { label: "Shipping" },
];

export default function ShippingPage() {
  return (
    <PageLayout
      title="Shipping"
      subtitle="Clear delivery options, tracking from the studio, and prepaid duties on international orders."
      crumbs={CRUMBS}
    >
      <section className="info-section">
        <h2>Shipping methods</h2>
        <p className="info-lead">
          Rates are shown at checkout based on your postcode. Business days exclude weekends and public holidays.
        </p>
        <div className="info-table-wrap">
          <table className="info-table">
            <caption className="visually-hidden">
              Shipping methods, prices, delivery times, and free-shipping thresholds
            </caption>
            <thead>
              <tr>
                <th scope="col">Method</th>
                <th scope="col">Price</th>
                <th scope="col">Delivery time</th>
                <th scope="col">Free shipping</th>
              </tr>
            </thead>
            <tbody>
              {METHODS.map((method) => (
                <tr key={method.name}>
                  <th scope="row">{method.name}</th>
                  <td>{method.price}</td>
                  <td>{method.delivery}</td>
                  <td>{method.threshold}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>

      <section className="info-section">
        <div className="info-grid cols-3">
          {INFO_CARDS.map((card) => {
            const Icon = ICONS[card.icon];
            return (
              <article className="info-card" key={card.title}>
                <span className="info-icon">
                  <Icon size={18} aria-hidden="true" />
                </span>
                <h3>{card.title}</h3>
                <p>{card.body}</p>
              </article>
            );
          })}
        </div>
      </section>

      <section className="info-section">
        <h2>How your order travels</h2>
        <div className="info-timeline">
          {TIMELINE.map((step, index) => (
            <article className="info-step" key={step.title}>
              <div className="info-step-index" aria-hidden="true">
                {index + 1}
              </div>
              <div>
                <h3>{step.title}</h3>
                <p>{step.body}</p>
              </div>
            </article>
          ))}
        </div>
      </section>

      <aside className="info-note">
        <h3>Please note</h3>
        <ul>
          {NOTES.map((note) => (
            <li key={note}>{note}</li>
          ))}
        </ul>
      </aside>
    </PageLayout>
  );
}
