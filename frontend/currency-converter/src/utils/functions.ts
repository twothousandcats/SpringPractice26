export function concatClassNames(
    classNames: (string | undefined | null | boolean)[]
): string {
    return classNames.filter(Boolean).join(' ');
}

export function formatNumber(value: number): string {
    return Number.isInteger(value) ? String(value) : value.toFixed(4);
}