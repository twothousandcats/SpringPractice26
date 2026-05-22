import styles from "./SelectList.module.scss";
import type { Currencies, Currency } from "../../models/types.ts";
import { SelectListItem } from "../SelectListItem/SelectListItem.tsx";

type SelectListProps = {
    currencies: Currencies;
    selected: string;
    onSelect: (code: string) => void;
}

export const SelectList = ({ currencies, selected, onSelect }: SelectListProps) => {
    return (
        <ul className={styles.list}>
            {currencies.map((item: Currency) => (
                <SelectListItem
                    key={item.code}
                    currencyCode={item.code}
                    isActive={item.code === selected}
                    onSelect={onSelect}
                />
            ))}
        </ul>
    );
}