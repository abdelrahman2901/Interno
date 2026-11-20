import fs from "fs";

const files = process.argv.slice(2);
if (files.length === 0) {
  console.error("Usage: node scripts/strip-api-prefix.mjs <generated-client.ts>...");
  process.exit(1);
}

for (const file of files) {
  if (!fs.existsSync(file)) {
    console.error(`Generated client not found: ${file}`);
    process.exit(1);
  }

  const source = fs.readFileSync(file, "utf8");
  const patched = source.replaceAll('this.baseUrl + "/api/', 'this.baseUrl + "/');
  fs.writeFileSync(file, patched);
  console.log(`Stripped /api prefix in ${file}`);
}
