namespace BuildingBlocks.Application.Ordering;

public static class DisplayOrderManager
{
    public static void Place<T>(
        IReadOnlyCollection<T> orderedItems,
        T item,
        int requestedPosition,
        Action<T, int> setDisplayOrder)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(
            orderedItems);

        ArgumentNullException.ThrowIfNull(item);

        ArgumentNullException.ThrowIfNull(
            setDisplayOrder);

        if (requestedPosition < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(requestedPosition));
        }

        List<T> reorderedItems =
            orderedItems
                .Where(current =>
                    !ReferenceEquals(
                        current,
                        item))
                .ToList();

        int targetPosition =
            Math.Min(
                requestedPosition,
                reorderedItems.Count);

        reorderedItems.Insert(
            targetPosition,
            item);

        ApplyOrder(
            reorderedItems,
            setDisplayOrder);
    }

    public static void Remove<T>(
        IReadOnlyCollection<T> orderedItems,
        T item,
        Action<T, int> setDisplayOrder)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(
            orderedItems);

        ArgumentNullException.ThrowIfNull(item);

        ArgumentNullException.ThrowIfNull(
            setDisplayOrder);

        List<T> remainingItems =
            orderedItems
                .Where(current =>
                    !ReferenceEquals(
                        current,
                        item))
                .ToList();

        ApplyOrder(
            remainingItems,
            setDisplayOrder);
    }

    private static void ApplyOrder<T>(
        IReadOnlyList<T> orderedItems,
        Action<T, int> setDisplayOrder)
    {
        for (
            int index = 0;
            index < orderedItems.Count;
            index++)
        {
            setDisplayOrder(
                orderedItems[index],
                index);
        }
    }
}