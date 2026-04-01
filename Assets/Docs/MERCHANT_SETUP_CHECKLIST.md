# Merchant Setup Checklist

Detected merchant/shop assets in the project:

- Prefab: `Assets/Prefabs/Character/Merchant_AI_Dummy_01.prefab`
  - shopName: `Roundtable Merchant`
  - merchantID: `merchant_ai_dummy_01`
  - autoScaleShopTierFromProgression: `true`
  - shopTierOffset: `0`
  - useGlobalPurchasableItems: `true`

Per merchant, verify these fields in `ShopInventory`:
- `merchantID`: unique stable id
- `autoScaleShopTierFromProgression`: enabled if shop should follow map tier
- `shopTierOffset`: `0` for normal, `1` for stronger/later stock, `-1` for cheaper/earlier stock
- `customStock`: fill if merchant should not sell the full global list
- `requiredProgressionTier`: set per item for unlock pacing

Suggested first pass for this prefab:
- Keep `merchantID = merchant_ai_dummy_01`
- Keep `autoScaleShopTierFromProgression = true`
- Keep `shopTierOffset = 0`
- Turn off `useGlobalPurchasableItems` once you are ready to curate a real merchant inventory
- Add early consumables at tier 1, stronger gear/materials at tiers 2-4
