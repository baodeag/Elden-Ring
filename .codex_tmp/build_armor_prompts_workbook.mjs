import fs from "node:fs/promises";
import path from "node:path";
import { Workbook, SpreadsheetFile } from "@oai/artifact-tool";

const workspaceRoot = process.cwd();
const armorRoot = path.join(workspaceRoot, "Assets", "Game", "Data", "Items", "Armor", "Polygon Armor Collection");
const outputDir = path.join(workspaceRoot, "outputs");
const outputPath = path.join(outputDir, "armor_render_prompts.xlsx");

const slotConfigs = {
  Head: {
    slotHint: "head armor",
    referenceSheet: "Temp/ArmorPreviews/Head.png",
    prompt:
      "Create a single game inventory icon PNG of '{ITEM_NAME}'. Use the attached armor preview as a strict reference. Recreate the exact same head armor silhouette as closely as possible. Match the exact crown height, brim width, visor cut, face opening, hood volume, plume placement, horn placement, wrap placement, and color placement from the reference. If the item is asymmetrical, preserve the same asymmetry. Match the exact color blocking visible in the reference preview. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only. Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated head armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, fabric weave, chainmail texture, scratches, ornate filigree, extra feathers, extra horns, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference. Use hard clean edges, crisp low-poly planes, and no soft airbrush shading."
  },
  Body: {
    slotHint: "torso armor",
    referenceSheet: "Temp/ArmorPreviews/Body.png",
    prompt:
      "Create a single game inventory icon PNG of '{ITEM_NAME}'. Use the attached armor preview as a strict reference. Recreate the exact same torso armor silhouette as closely as possible. Match the exact shoulder width, torso length, hem shape, collar shape, sleeve cutoff, layered panel placement, cloth drape blocks, chest plate blocks, belt placement, fur placement, and color placement from the reference. Preserve the same split between cloth, leather, and metal masses visible in the preview. Match the exact color blocking visible in the reference preview. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only. Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated chest armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, stitching detail, fabric weave, extra pouches, extra straps, extra metal trim, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference. Use hard clean edges, crisp low-poly planes, and no soft airbrush shading."
  },
  Hands: {
    slotHint: "hand armor",
    referenceSheet: "Temp/ArmorPreviews/Hands.png",
    prompt:
      "Create a single game inventory icon PNG of '{ITEM_NAME}'. Use the attached armor preview as a strict reference. Recreate the exact same hand armor silhouette as closely as possible. Match the exact glove length, cuff width, plate segmentation, knuckle massing, wrap thickness, finger block shape, wrist silhouette, and color placement from the reference. Preserve whether the item reads as cloth wraps, leather gloves, or metal gauntlets. Match the exact color blocking visible in the reference preview. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only. Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated hand armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, seam detail, extra finger joints, extra spikes, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference. Use hard clean edges, crisp low-poly planes, and no soft airbrush shading."
  },
  Legs: {
    slotHint: "leg armor",
    referenceSheet: "Temp/ArmorPreviews/Legs.png",
    prompt:
      "Create a single game inventory icon PNG of '{ITEM_NAME}'. Use the attached armor preview as a strict reference. Recreate the exact same leg armor silhouette as closely as possible. Match the exact waist block, skirt flap arrangement, thigh width, shin width, boot height, knee massing, side panel placement, cloth layering, armor plate placement, and color placement from the reference. Preserve whether the item reads as cloth trousers, leather leggings, plated greaves, or split tassets. Match the exact color blocking visible in the reference preview. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only. Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated leg armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, cloth folds beyond the reference, extra straps, extra buckles, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference. Use hard clean edges, crisp low-poly planes, and no soft airbrush shading."
  }
};

async function listAssetFiles(dirPath) {
  const entries = await fs.readdir(dirPath, { withFileTypes: true });
  const files = [];
  for (const entry of entries) {
    const fullPath = path.join(dirPath, entry.name);
    if (entry.isDirectory()) {
      files.push(...await listAssetFiles(fullPath));
      continue;
    }

    if (entry.isFile() && entry.name.endsWith(".asset")) {
      files.push(fullPath);
    }
  }

  return files;
}

async function parseArmorItem(assetAbsolutePath) {
  const content = await fs.readFile(assetAbsolutePath, "utf8");
  const itemNameMatch = content.match(/^\s*itemName:\s*(.+)$/m);
  const name = itemNameMatch ? itemNameMatch[1].trim() : path.basename(assetAbsolutePath, ".asset");
  const normalizedPath = assetAbsolutePath.replaceAll("\\", "/");
  const slot = normalizedPath.includes("/Head/") ? "Head"
    : normalizedPath.includes("/Body/") ? "Body"
    : normalizedPath.includes("/Hands/") ? "Hands"
    : normalizedPath.includes("/Legs/") ? "Legs"
    : "Unknown";

  return {
    itemName: name,
    slot,
    assetPath: path.relative(workspaceRoot, assetAbsolutePath).replaceAll("\\", "/"),
  };
}

function buildPrompt(itemName, slot) {
  const config = slotConfigs[slot];
  if (!config) {
    return `Create a single game inventory icon PNG of '${itemName}'. Use the attached armor preview as a strict reference. Match the exact shape, proportions, color blocking, and low-poly plane structure from the reference. Transparent background only, one isolated item only, no character, no environment, and no added detail.`;
  }

  return config.prompt.replaceAll("{ITEM_NAME}", itemName);
}

const assetFiles = (await listAssetFiles(armorRoot))
  .sort((a, b) => a.localeCompare(b));

const items = [];
for (const assetFile of assetFiles) {
  items.push(await parseArmorItem(assetFile));
}

const workbook = Workbook.create();
const overview = workbook.worksheets.add("Overview");
const promptsSheet = workbook.worksheets.add("Armor Prompts");

const slotCounts = ["Head", "Body", "Hands", "Legs"].map(slot => [
  slot,
  items.filter(item => item.slot === slot).length
]);

overview.getRange("A1:B8").values = [
  ["Armor Render Prompts", "Generated workbook"],
  ["Generated On", new Date().toISOString().slice(0, 10)],
  ["Total Items", items.length],
  ["", ""],
  ["Slot", "Count"],
  ...slotCounts
];

const promptRows = items.map((item, index) => [
  index + 1,
  item.slot,
  item.itemName,
  item.assetPath,
  slotConfigs[item.slot]?.referenceSheet ?? "",
  buildPrompt(item.itemName, item.slot)
]);

promptsSheet.getRange(`A1:F${promptRows.length + 1}`).values = [
  ["ID", "Slot", "Item Name", "Asset Path", "Reference Preview", "Full Prompt"],
  ...promptRows
];

const summary = {
  totalItems: items.length,
  head: slotCounts.find(row => row[0] === "Head")?.[1] ?? 0,
  body: slotCounts.find(row => row[0] === "Body")?.[1] ?? 0,
  hands: slotCounts.find(row => row[0] === "Hands")?.[1] ?? 0,
  legs: slotCounts.find(row => row[0] === "Legs")?.[1] ?? 0
};

await fs.mkdir(outputDir, { recursive: true });
const exported = await SpreadsheetFile.exportXlsx(workbook);
await exported.save(outputPath);

console.log(JSON.stringify({ outputPath, summary }, null, 2));
