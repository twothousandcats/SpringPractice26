import styles from "./SelectList.module.scss";
import type {Currencies, Currency} from "../../models/types.ts";
import {SelectListItem} from "../SelectListItem/SelectListItem.tsx";
import {concatClassNames} from "../../utils/functions.ts";

type SelectListProps = {
    currencies: Currencies;
    selected: string;
    isOpen: boolean;
    onSelect: (code: string) => void;
}

export const SelectList = (
    {
        currencies,
        selected,
        isOpen,
        onSelect,
    }: SelectListProps) => {
    const classNames = concatClassNames([
        styles.list,
        isOpen && styles["list--show"]
    ]);

    return (
        <ul className={classNames}>
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