import moment from 'moment';

function formatDateTime(date) {
  return moment(date)
    .format('YYYY-MM-DD HH:mm:ss');
}
export {
  formatDateTime
};