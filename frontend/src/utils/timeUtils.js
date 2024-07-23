export const getTimeDifference = (date) => {
    const now = new Date();
    const diffInSeconds = Math.floor((now - new Date(date)) / 1000);
  
    const units = [
      { name: 'year', seconds: 31536000 },
      { name: 'month', seconds: 2592000 },
      { name: 'week', seconds: 604800 },
      { name: 'day', seconds: 86400 },
      { name: 'hour', seconds: 3600 },
      { name: 'minute', seconds: 60 },
      { name: 'second', seconds: 1 },
    ];
  
    for (let unit of units) {
      const count = Math.floor(diffInSeconds / unit.seconds);
      if (count >= 1) {
        return `${count} ${unit.name}${count > 1 ? 's' : ''} ago`;
      }
    }
  
    return 'just now';
  };
  