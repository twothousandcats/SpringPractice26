import styles from "./SelectList.module.scss";
import type {Currencies, Currency} from "../../store/types/types.ts";
import {SelectListItem} from "../SelectListItem/SelectListItem.tsx";

type SelectListProps = {
    currencies?: Currencies,
}

export const SelectList = ({currencies}: SelectListProps) => {
    return (
        <ul className={styles.list}>
            {currencies && currencies.map((item: Currency, idx: number) => (
                <SelectListItem
                    key={idx}
                    currencyCode={item.code}
                    isActive={false}
                />
            ))}
        </ul>
    );
}