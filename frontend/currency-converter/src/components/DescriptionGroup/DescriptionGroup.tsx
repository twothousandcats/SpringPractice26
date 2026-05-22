import {useState} from "react";
import styles from './DescriptionGroup.module.scss';
import type {Currency} from "../../models/types.ts";
import {MoreButton} from "../MoreButton/MoreButton.tsx";
import {DescriptionLine} from "../DescriptionLine/DescriptionLine.tsx";

type DescriptionGroupProps = {
    from: Currency;
    to: Currency;
}

export const DescriptionGroup = ({from, to}: DescriptionGroupProps) => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <div className={styles.descriptionGroup}>
            <div className={styles.btnContainer}>
                <MoreButton
                    title={`${from.code}/${to.code}: about`}
                    isOpen={isOpen}
                    onToggle={
                        () => setIsOpen((open) => !open)
                    }
                />
            </div>
            {isOpen && (
                <div className={styles.descriptionLinesSeparator}>
                    <DescriptionLine currency={from}/>
                    <DescriptionLine currency={to}/>
                </div>
            )}
        </div>
    );
}