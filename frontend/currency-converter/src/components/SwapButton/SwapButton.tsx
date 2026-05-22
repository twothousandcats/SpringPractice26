import styles from './SwapButton.module.scss';

type SwapButtonProps = {
    onSwap: () => void;
}

export const SwapButton = (
    {
        onSwap,
    }: SwapButtonProps
) => {
    return (
        <button
            type="button"
            className={styles.btn}
            onClick={onSwap}>
            Swap
        </button>
    );
}