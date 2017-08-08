extern "C"
{
	bool hasTwitch() 
	{
		NSURL *twitchURL = [NSURL URLWithString:@"twitch://open"];
		if ([[UIApplication sharedApplication] canOpenURL:twitchURL]) {
			// The Twitch app is installed, do whatever logic you need, and call -openURL:
			return true;
		} else {
			// The Twitch app is not installed. Prompt the user to install it!
			return false;
		}
	}    
}