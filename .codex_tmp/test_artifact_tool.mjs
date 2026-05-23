import { Workbook, SpreadsheetFile } from "@oai/artifact-tool";
import fs from "node:fs/promises";

const workbook = Workbook.create();
const sheet = workbook.worksheets.add("Test");
sheet.getRange("A1:B3").values = [
  ["Name", "Prompt"],
  ["Item 1", "Hello"],
  ["Item 2", "World"],
];

const outDir = new URL("./out/", import.meta.url);
await fs.mkdir(outDir, { recursive: true });
const output = await SpreadsheetFile.exportXlsx(workbook);
await output.save(new URL("./out/test.xlsx", import.meta.url));
console.log("done");
