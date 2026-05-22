import styles from './SelectListItem.module.scss';
import {concatClassNames} from '../../utils/functions.ts';

type SelectListItemProps = {
    currencyCode: string;
    isActive: boolean;
    onSelect: (code: string) => void;
}

export const SelectListItem = ({currencyCode, isActive, onSelect}: SelectListItemProps) => {
    return (
        <li>
            <button
                type="button"
                aria-pressed={isActive}
                className={concatClassNames([
                    styles.selectListItem,
                    isActive && styles.active,
                ])}
                onClick={() => onSelect(currencyCode)}
            >
                {currencyCode}
            </button>
        </li>
    );
}