export const concatClassNames = (
    classNames: (string | undefined | null | boolean)[]
): string => {
    return classNames.filter(Boolean).join(' ');
}

export const formatNumber = (value: number): string => {
    return Number.isInteger(value)
        ? String(value)
        : value.toFixed(4);
}

export function formatDateTime(iso: string): string {
    if (!iso) {
        return '';
    }

    const date = new Date(iso);
    if (isNaN(date.getTime())) {
        return 'Invalid Date';
    }
    const days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    const dayName = days[date.getUTCDay()];
    const day = String(date.getUTCDate()).padStart(2, '0');
    const month = months[date.getUTCMonth()];
    const year = date.getUTCFullYear();
    const hours = String(date.getUTCHours()).padStart(2, '0');
    const minutes = String(date.getUTCMinutes()).padStart(2, '0');

    return `${dayName}, ${day} ${month} ${year} ${hours}:${minutes} UTC`;
}