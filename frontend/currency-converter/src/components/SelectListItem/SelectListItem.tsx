import styles from './SelectListItem.module.scss';

type SelectListItemProps = {
    key: number;
    currencyCode: string,
    isActive: boolean,
}

export const SelectListItem = (
    {
        key,
        currencyCode,
        isActive,
    }: SelectListItemProps
) => {
    return (
        <li className={styles.selectListItem}>
            {currencyCode}
        </li>
    );
}