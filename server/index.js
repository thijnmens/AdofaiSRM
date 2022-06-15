const request = require('request');
const tmi = require('tmi.js');
const WebSocketServer = require('ws').Server;

const socket = new WebSocketServer({ port: 666 });

socket.on('connection', function connection(ws) {
	client.on('message', (channel, tags, message, self) => {
		// Ignore echoed messages.
		if (self || !message.startsWith('>')) return;

		const args = message.slice(1).split(' ');
		const command = args.shift().toLowerCase();

		switch (command.toLowerCase()) {
			case 'hello':
				client.say(channel, `Hello @${tags.username}!`);
				break;

			case 'srm':
				request(
					`https://adofai.gg:9200/api/v1/levels/${args[0]}`,
					{ json: true },
					(err, res, body) => {
						if (err) {
							return console.log(err);
						}
						ws.send(
							JSON.stringify({
								type: 'queueAdd',
								data: res.body,
							})
						);
						if (res.body.workshop === null) {
							client.say(
								channel,
								`@${tags.username},"${res.body.title}" is not uploaded to the steam workshop, non-workshop levels are not yet supported`
							);
						} else {
							client.say(
								channel,
								`@${tags.username}, I've successfully added "${res.body.title}" to the queue!`
							);
						}
					}
				);
				break;

			default:
				break;
		}
	});
	ws.send(
		JSON.stringify({
			type: 'open',
			data: {
				success: true,
			},
		})
	);
});

const client = new tmi.Client({
	options: { debug: true },
	identity: {
		username: 'AdofaiSRM',
		password: 'bruh', // https://twitchapps.com/tmi/
	},
	channels: ['thijnmens'],
});

client.connect();
