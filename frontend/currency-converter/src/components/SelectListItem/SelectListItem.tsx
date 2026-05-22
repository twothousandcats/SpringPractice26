import styles from './SelectListItem.module.scss';

type SelectListItemProps = {
    currencyCode: string;
    isActive: boolean;
    onSelect: (code: string) => void;
}

export const SelectListItem = ({currencyCode, isActive, onSelect}: SelectListItemProps) => {
    return (
        <li className={styles.selectListItem}
            aria-pressed={isActive}
            onClick={() => onSelect(currencyCode)}>
            {currencyCode}
        </li>
    );
}