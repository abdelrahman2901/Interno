import fs from "fs";
import path from "path";
import { fileURLToPath } from "url";

const root = path.join(path.dirname(fileURLToPath(import.meta.url)), "..");
const outputPath = path.join(
  root,
  "src",
  "Core",
  "API",
  "Interno-Microservice-Client",
  "Interno-Microservice-Client.ts",
);

function patchMethodBlock(source, methodStart, methodEndMarker, replacements) {
  const start = source.indexOf(methodStart);
  if (start === -1) {
    console.warn(`Method start not found: ${methodStart}`);
    return source;
  }

  const end = source.indexOf(methodEndMarker, start);
  if (end === -1) {
    console.warn(`Method end not found after ${methodStart}: ${methodEndMarker}`);
    return source;
  }

  let block = source.slice(start, end);
  for (const [pattern, replacement] of replacements) {
    block = block.replace(pattern, replacement);
  }

  return source.slice(0, start) + block + source.slice(end);
}

function patchOptionalParam(source, paramName, formKey) {
  const requiredPattern = new RegExp(
    `if \\(${paramName} === null \\|\\| ${paramName} === undefined\\)\\s*throw new globalThis\\.Error\\("The parameter '${paramName}' cannot be null\\."\\);\\s*else\\s*content_\\.append\\("${formKey}", ${paramName}\\.toString\\(\\)\\);`,
    "g",
  );

  return source.replace(
    requiredPattern,
    `if (${paramName} !== null && ${paramName} !== undefined)\n            content_.append("${formKey}", ${paramName}.toString());`,
  );
}

function patchNullableColorFields(source) {
  let patched = source;

  patched = patchMethodBlock(patched, "createBanner(", "processCreateBanner(", [
    [
      /backgroundColorID: string \| undefined, backgroundColorHex: string \| undefined, accentColorID: string \| undefined, accentColorHex: string \| undefined/,
      "backgroundColorID: string | null | undefined, backgroundColorHex: string | null | undefined, accentColorID: string | null | undefined, accentColorHex: string | null | undefined",
    ],
  ]);

  patched = patchMethodBlock(patched, "updateBanner(", "processUpdateBanner(", [
    [
      /backgroundColorID: string \| undefined, backgroundColorHex: string \| undefined, accentColorID: string \| undefined, accentColorHex: string \| undefined/,
      "backgroundColorID: string | null | undefined, backgroundColorHex: string | null | undefined, accentColorID: string | null | undefined, accentColorHex: string | null | undefined",
    ],
  ]);

  patched = patchOptionalParam(patched, "backgroundColorID", "BackgroundColorID");
  patched = patchOptionalParam(patched, "backgroundColorHex", "BackgroundColorHex");
  patched = patchOptionalParam(patched, "accentColorID", "AccentColorID");
  patched = patchOptionalParam(patched, "accentColorHex", "AccentColorHex");

  return patched;
}

if (!fs.existsSync(outputPath)) {
  console.error(`Generated client not found: ${outputPath}`);
  process.exit(1);
}

let generated = fs.readFileSync(outputPath, "utf8");
generated = patchNullableColorFields(generated);
fs.writeFileSync(outputPath, generated);

console.log("Patched optional banner color fields in Interno-Microservice-Client.ts");
