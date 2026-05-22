import styles from './MoreButton.module.scss';

type MoreButtonProps = {
    title: string;
    isOpen: boolean;
    onToggle: () => void;
}

export const MoreButton = ({title, isOpen, onToggle}: MoreButtonProps) => {
    return (
        <button
            type="button"
            className={styles.moreButton}
            aria-expanded={isOpen}
            onClick={onToggle}
        >
            {title}
        </button>
    );
}