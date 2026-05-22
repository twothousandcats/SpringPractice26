import styles from './MoreButton.module.scss';
import {ArrowUpIcon} from "../Icons/ArrowUpIcon.tsx";
import {concatClassNames} from "../../utils/functions.ts";

type MoreButtonProps = {
    title: string;
    isOpen: boolean;
    onToggle: () => void;
}

export const MoreButton = ({title, isOpen, onToggle}: MoreButtonProps) => {
    const classNames = concatClassNames([
        styles["more-button"],
        isOpen && styles["more-button--show"],
    ]);
    return (
        <button
            type="button"
            className={classNames}
            aria-expanded={isOpen}
            onClick={onToggle}
        >
            {title}
            <ArrowUpIcon/>
        </button>
    );
}