import http from 'k6/http';
import {check} from 'k6';

export const options = {
    vus: 10,
    duration: '1m',
    thresholds: {
        'http_req_duration': ['p(99)<10']
    }
};

export default function () {
    const res = http.get('http://localhost:5279/api/benchmark/22');

    check(res, {
        'status was 200': (r) => r.status === 200
    });
}