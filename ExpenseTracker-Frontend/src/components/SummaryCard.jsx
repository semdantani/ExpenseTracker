const SummaryCard = ({ title, value, icon }) => {
  return (
    <div className="summary-card">
      <div>
        <p>{title}</p>
        <h2>{value}</h2>
      </div>

      <div className="summary-icon">{icon}</div>
    </div>
  );
};

export default SummaryCard;
