# Armor Render Prompts

Tai lieu nay dung de tao icon giap theo cung style voi weapon icon, nhung khoa chat hon vao silhouette, block mau, va do don gian low-poly cua model armor hien co.

Project da co code doc/render giap de lam preview:

- [PolygonArmorPreviewExporter.cs](/D:/UnitySetup/Project/Elden%20Ring/Assets/Game/Editor/PolygonArmorPreviewExporter.cs)

Tool nay render giap tu `ArmorItem.equipmentModels` bang cach bat dung mesh tren `Player.prefab`, vi vay ban co the dung preview armor xuat ra lam reference goc cho AI.

## Cach dung

1. Trong Unity, chay `Tools/Export Polygon Armor Previews`.
2. Mo `Temp/ArmorPreviews/Body.png`, `Head.png`, `Hands.png`, `Legs.png`.
3. Cat item can dung thanh mot anh reference rieng.
4. Paste prompt phu hop ben duoi.
5. Luon gui kem anh preview vua cat cho model AI.

## Global Rules

Dung doan nay o cuoi moi prompt:

```text
Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, fabric weave, scratches, ornate filigree, extra straps, extra buckles, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference.
```

## Master Prompt

Thay:

- `{ITEM_NAME}` bang ten item
- `{SLOT_HINT}` bang `helmet`, `chest armor`, `gloves`, `gauntlets`, `leggings`, `greaves`...
- `{COLOR_HINT}` bang mo ta palette that su thay trong preview, vi du `steel gray and dark red cloth` hoac `brown leather with muted blue cloth`

```text
Create a single game inventory icon PNG of '{ITEM_NAME}'. Use the attached armor preview as a strict reference. Recreate the exact same {SLOT_HINT} shape and silhouette as closely as possible. Match the exact outer contour, proportions, panel layout, cloth placement, plate placement, trim placement, and color placement from the reference. Overall palette reads as {COLOR_HINT}. Keep these same flat color families in the same visible places as the reference. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only.

Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, fabric weave, scratches, ornate filigree, extra straps, extra buckles, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference.
```

## Head Prompt

Dung cho helm, hood, hat, circlet, mask.

```text
Create a single game inventory icon PNG of '{ITEM_NAME}'. Use the attached armor preview as a strict reference. Recreate the exact same head armor silhouette as closely as possible. Match the exact crown height, brim width, visor cut, face opening, hood volume, plume placement, horn placement, wrap placement, and color placement from the reference. If the item is asymmetrical, preserve the same asymmetry. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only.

Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated head armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, fabric weave, chainmail texture, scratches, ornate filigree, extra feathers, extra horns, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference.
```

## Body Prompt

Dung cho cuirass, coat, jerkin, tabard, vestments, brigandine.

```text
Create a single game inventory icon PNG of '{ITEM_NAME}'. Use the attached armor preview as a strict reference. Recreate the exact same torso armor silhouette as closely as possible. Match the exact shoulder width, torso length, hem shape, collar shape, sleeve cutoff, layered panel placement, cloth drape blocks, chest plate blocks, belt placement, fur placement, and color placement from the reference. Preserve the same split between cloth, leather, and metal masses visible in the preview. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only.

Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated chest armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, stitching detail, fabric weave, extra pouches, extra straps, extra metal trim, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference.
```

## Hands Prompt

Dung cho gloves, wraps, gauntlets.

```text
Create a single game inventory icon PNG of '{ITEM_NAME}'. Use the attached armor preview as a strict reference. Recreate the exact same hand armor silhouette as closely as possible. Match the exact glove length, cuff width, plate segmentation, knuckle massing, wrap thickness, finger block shape, wrist silhouette, and color placement from the reference. Preserve whether the item reads as cloth wraps, leather gloves, or metal gauntlets. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only.

Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated hand armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, seam detail, extra finger joints, extra spikes, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference.
```

## Legs Prompt

Dung cho boots, leggings, chausses, greaves, skirts, tassets, trousers.

```text
Create a single game inventory icon PNG of '{ITEM_NAME}'. Use the attached armor preview as a strict reference. Recreate the exact same leg armor silhouette as closely as possible. Match the exact waist block, skirt flap arrangement, thigh width, shin width, boot height, knee massing, side panel placement, cloth layering, armor plate placement, and color placement from the reference. Preserve whether the item reads as cloth trousers, leather leggings, plated greaves, or split tassets. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only.

Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated leg armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, cloth folds beyond the reference, extra straps, extra buckles, glow, particles, or painterly texture. Do not invent a new palette. Preserve the same proportions and the same number of major shape masses visible in the reference.
```

## Fast Examples

### Knightly Helm

```text
Create a single game inventory icon PNG of 'Knightly Helm'. Use the attached armor preview as a strict reference. Recreate the exact same head armor silhouette as closely as possible. Match the exact crown height, visor cut, face opening, neck guard mass, and color placement from the reference. Overall palette reads as steel gray with darker shadowed steel accents. Keep these same flat color families in the same visible places as the reference. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only. Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated head armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, chainmail texture, scratches, ornate filigree, extra horns, glow, particles, or painterly texture. Do not invent a new palette.
```

### Bastion Cuirass

```text
Create a single game inventory icon PNG of 'Bastion Cuirass'. Use the attached armor preview as a strict reference. Recreate the exact same torso armor silhouette as closely as possible. Match the exact shoulder width, chest block shape, hem silhouette, layered panel placement, and color placement from the reference. Overall palette reads as steel gray armor with muted cloth accents. Keep these same flat color families in the same visible places as the reference. Keep the item in an extremely simple low-poly style, with very few large planes, very little micro detail, minimal surface noise, and broad flat color areas only. Transparent background only. Output a PNG with no background, no backdrop, no frame, no environment, no character body, no face, no skin, no mannequin, no hand model, no text, no watermark. Keep one isolated chest armor item only, centered in the canvas, fully visible. Match the exact low-poly silhouette and the same color blocking from the reference. Keep the same large flat planes, the same simple material read, and the same apparent pixel-size density as the source game model. Do not add realism, stitching detail, extra straps, extra trim, glow, particles, or painterly texture. Do not invent a new palette.
```

## Goi y de ra icon giong weapon hon

- Canvas nen de vuong: `512x512` hoac `1024x1024`, xuat `PNG`.
- Neu model AI hay lam mem hinh, them cau: `hard clean edges, crisp low-poly planes, no soft airbrush shading`.
- Neu item bi ve kem nguoi/mannequin, them cau: `armor item only, no wearer, no bust, no body parts`.
- Neu mau bi lech, them mot dong rieng: `match the exact color blocking from the attached preview, especially the primary cloth and metal zones`.
- Neu item bi them chi tiet, them mot dong rieng: `reduce detail to the same level as the source game mesh`.
