import styles from './MethodBadge.module.css';

interface Props {
  method: string;
}

export function MethodBadge({ method }: Props) {
  return (
    <span className={`${styles.badge} ${styles[method.toLowerCase()] ?? ''}`}>
      {method}
    </span>
  );
}