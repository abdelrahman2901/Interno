import { useMemo, useState } from "react";
import PageLayout, { PageCrumb } from "./PageLayout";

type SizeTab = "Men" | "Women" | "Kids";

type SizeRow = {
  size: string;
  chest: number;
  waist: number;
  hip: number;
  height: number;
};

type MeasureTip = {
  title: string;
  body: string;
};

const TABS: SizeTab[] = ["Men", "Women", "Kids"];

const SIZE_CHARTS: Record<SizeTab, SizeRow[]> = {
  Women: [
    { size: "XS", chest: 82, waist: 64, hip: 90, height: 160 },
    { size: "S", chest: 86, waist: 68, hip: 94, height: 164 },
    { size: "M", chest: 92, waist: 74, hip: 100, height: 168 },
    { size: "L", chest: 98, waist: 80, hip: 106, height: 172 },
    { size: "XL", chest: 106, waist: 88, hip: 114, height: 176 },
    { size: "XXL", chest: 114, waist: 96, hip: 122, height: 178 },
  ],
  Men: [
    { size: "S", chest: 92, waist: 78, hip: 94, height: 170 },
    { size: "M", chest: 98, waist: 84, hip: 100, height: 176 },
    { size: "L", chest: 106, waist: 92, hip: 106, height: 182 },
    { size: "XL", chest: 114, waist: 100, hip: 112, height: 186 },
    { size: "XXL", chest: 124, waist: 110, hip: 120, height: 190 },
  ],
  Kids: [
    { size: "2-3Y", chest: 54, waist: 52, hip: 56, height: 98 },
    { size: "4-5Y", chest: 58, waist: 54, hip: 62, height: 110 },
    { size: "6-7Y", chest: 64, waist: 58, hip: 68, height: 122 },
    { size: "8-9Y", chest: 70, waist: 62, hip: 74, height: 134 },
    { size: "10-12Y", chest: 78, waist: 66, hip: 82, height: 148 },
  ],
};

const MEASURE_TIPS: MeasureTip[] = [
  {
    title: "Chest",
    body: "Measure around the fullest part of the chest, keeping the tape level and relaxed.",
  },
  {
    title: "Waist",
    body: "Find the natural waist, usually the narrowest point. Do not pull the tape tight.",
  },
  {
    title: "Hip and height",
    body: "Measure hips at the widest point. For height, stand against a wall without shoes.",
  },
];

const CRUMBS: PageCrumb[] = [
  { label: "Home", to: "/Home" },
  { label: "Size Guide" },
];

function toInches(cm: number) {
  return Math.round((cm / 2.54) * 10) / 10;
}

function formatValue(cm: number, unit: "cm" | "in") {
  return unit === "cm" ? `${cm} cm` : `${toInches(cm)} in`;
}

export default function SizeGuidePage() {
  const [tab, setTab] = useState<SizeTab>("Women");
  const [unit, setUnit] = useState<"cm" | "in">("cm");
  const rows = useMemo(() => SIZE_CHARTS[tab], [tab]);

  return (
    <PageLayout
      title="Size guide"
      subtitle="Find your Interno fit. Charts are garment body measurements, not branded vanity sizes."
      crumbs={CRUMBS}
    >
      <section className="info-section">
        <div className="info-tabs" role="tablist" aria-label="Size charts">
          {TABS.map((item) => (
            <button
              key={item}
              type="button"
              role="tab"
              className={`info-tab ${tab === item ? "active" : ""}`}
              aria-selected={tab === item}
              onClick={() => setTab(item)}
            >
              {item}
            </button>
          ))}
        </div>

        <div className="unit-toggle" role="group" aria-label="Measurement unit">
          <button
            type="button"
            className={unit === "cm" ? "active" : ""}
            aria-pressed={unit === "cm"}
            onClick={() => setUnit("cm")}
          >
            cm
          </button>
          <button
            type="button"
            className={unit === "in" ? "active" : ""}
            aria-pressed={unit === "in"}
            onClick={() => setUnit("in")}
          >
            in
          </button>
        </div>

        <div className="info-table-wrap">
          <table className="info-table">
            <caption className="visually-hidden">
              {tab} size chart in {unit === "cm" ? "centimetres" : "inches"}
            </caption>
            <thead>
              <tr>
                <th scope="col">Size</th>
                <th scope="col">Chest</th>
                <th scope="col">Waist</th>
                <th scope="col">Hip</th>
                <th scope="col">Height</th>
              </tr>
            </thead>
            <tbody>
              {rows.map((row) => (
                <tr key={row.size}>
                  <th scope="row">{row.size}</th>
                  <td>{formatValue(row.chest, unit)}</td>
                  <td>{formatValue(row.waist, unit)}</td>
                  <td>{formatValue(row.hip, unit)}</td>
                  <td>{formatValue(row.height, unit)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>

      <section className="info-section">
        <h2>How to measure</h2>
        <p className="info-lead">
          Use a soft tape over light clothing. If you are between sizes, we recommend sizing up for coats and knits.
        </p>
        <div className="info-grid cols-3">
          {MEASURE_TIPS.map((tip) => (
            <article className="info-card" key={tip.title}>
              <h3>{tip.title}</h3>
              <p>{tip.body}</p>
            </article>
          ))}
        </div>
      </section>
    </PageLayout>
  );
}
