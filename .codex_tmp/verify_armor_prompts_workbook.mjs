import { FileBlob, SpreadsheetFile } from "@oai/artifact-tool";

const input = await FileBlob.load("outputs/armor_render_prompts.xlsx");
const workbook = await SpreadsheetFile.importXlsx(input);

const overview = await workbook.inspect({
  kind: "table",
  range: "Overview!A1:B9",
  include: "values",
  tableMaxRows: 12,
  tableMaxCols: 4
});

const prompts = await workbook.inspect({
  kind: "table",
  range: "Armor Prompts!A1:F6",
  include: "values",
  tableMaxRows: 6,
  tableMaxCols: 6
});

console.log(JSON.stringify({
  overview: overview.ndjson,
  prompts: prompts.ndjson
}, null, 2));
