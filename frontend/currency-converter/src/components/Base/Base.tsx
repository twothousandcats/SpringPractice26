import styles from "./Base.module.scss";
import {WorkArea} from "../WorkArea/WorkArea.tsx";
import {DescriptionGroup} from "../DescriptionGroup/DescriptionGroup.tsx";
import {useEffect, useState} from "react";
import type {Currencies} from "../../store/types/types.ts";
import {ENV_CONFIG} from "../../utils/config.ts";

export const Base = () => {
    const [currencies, setCurrencies] = useState<Currencies | undefined>(undefined);

    useEffect(() => {
        fetch(ENV_CONFIG.api.currency)
            .then(res => res.json())
            .then(data => setCurrencies(data));
    });
    return (
        <div className={`${styles.base} ${styles.container}`}>
            <WorkArea currencies={currencies}/>
            {currencies && (
                currencies.map((currency, i) => {
                    <DescriptionGroup currency={currency} key={i}/>
                })
            )}
        </div>
    )
}