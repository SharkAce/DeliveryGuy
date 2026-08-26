# Adding a New Delivery Route

Follow these steps whenever you add a delivery.

## 1. Add the route

1. Find the `DeliveryRoute` prefab in the Project window.
2. Drag it under `Deliveries` in the Hierarchy.
3. Rename it, for example `DeliveryRoute03`.

It should contain:

```text
DeliveryRoute03
├── PickUpPoint
└── DropOffPoint
```

## 2. Place the points

Move `PickUpPoint` to where the package will be collected.

Move `DropOffPoint` to where the package will be delivered.

Make sure both points are somewhere the car can reach. Each point should already have:

- A `Circle Collider 2D` with **Is Trigger** ticked.
- A `DeliveryPoint` script.

## 3. Enter the delivery information

Select the new `DeliveryRoute` and fill in:

- **Pickup Name** — the collection location shown on the phone.
- **Destination Name** — the delivery location shown on the phone.
- **Pickup Point** — drag in this route's `PickUpPoint`.
- **Drop Off Point** — drag in this route's `DropOffPoint`.
- **Boss Line** — optional message for this delivery.

The point fields may already be filled in by the prefab, but check that they use the new route's own points.

## 4. Add it to DeliveryManager

1. Select `DeliveryManager`.
2. Expand **Deliveries**.
3. Increase **Size** by one.
4. Drag the new route into the new slot.

For example:

```text
Element 0 → DeliveryRoute01
Element 1 → DeliveryRoute02
Element 2 → DeliveryRoute03
```

This list controls the order of the deliveries. Check that Unity has not copied the previous route into the new slot.

## 5. Add the minimap markers

Under `MinimapMarkers/DeliveryMarkers`:

1. Find the `DeliveryMarker` prefab in the Project window.
2. Drag it under `DeliveryMarkers` in the Hierarchy.
3. Rename it, for example `PickupMarker03`.
4. Do the same for the drop off point.

Select each new marker and set its `MinimapMarkerFollower` target:

```text
PickupMarker03  → DeliveryRoute03/PickUpPoint
DropOffMarker03 → DeliveryRoute03/DropOffPoint
```

Keep the markers separate from the delivery route. Their settings should be:

```text
Layer: Minimap
Sorting Layer: Minimap
Order in Layer: 10
```

Keep the X and Y scale the same so the dots stay circular.

## 6. Connect the markers

1. Select `DeliveryMarkers`.
2. Find the `MinimapDeliveryMarkers` script.
3. Increase **Delivery Markers Size** by one.
4. Open the new element.
5. Add the new pickup and drop-off markers.

For example:

```text
Element 2
├── Pickup Marker   → PickupMarker03
└── Drop Off Marker → DropOffMarker03
```

The element number must match the route's element number in `DeliveryManager`.

## 7. Do I need to change any code?

No. A normal new delivery does not require any code changes.

### Phone text

You do not need to change `PhoneUI`. It automatically reads the information from the current `DeliveryRoute`:

- **Pickup Name** appears as the collection location.
- **Destination Name** appears as the delivery location.
- **Boss Line** appears as an extra message if you enter one.
- The order number is worked out automatically from the delivery list.

The objective arrow automatically:

- Points to the current pickup.
- Switches to the drop-off after collection.
- Switches to the next route when the delivery is complete.
- Disappears after the final delivery.

The phone and minimap markers also update automatically.
