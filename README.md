# NightscoutHybridMobile
A cross platform mobile app that shows your Nightscout website in a WebView.

For more information about Nightscout and remote continuous glucose monitoring in the cloud, visit [nightscout.info](http://nightscout.info)

### Features:

1. Your full Nightscout website displayed inside the app
2. The ability to prevent the screen from locking so you can enjoy alarms throughout the night :)
3. Volume control in case you enjoy alarms throughout the day.
4. NEW: Push notifications for Alerts, Announcements, and Info events (customizable per device) so you can get alarms when the app is not running.  This is an alternative (FREE!) to Pushover, and works very similarly. 
5. NEW: Support for iOS and Android 
6. NEW: A flashlight so you can sneak into your child's room in the middle of the night for blood sugar checks and treatments.

We welcome your feedback and feature requests.  Please add them as [issues](https://github.com/aditmer/NightscoutHybridMobile/issues)

#Installing the mobile App 

If you would like to Beta Test this app, please join our [HockeyApp Team](https://rink.hockeyapp.net/recruit/460522d7157b4881a8e64adea9e15c74).  We are using HockyApp to distribute the beta.  If you have issues or questions on how to install this app, feel free to ask in our [Facebook Group](https://www.facebook.com/groups/347752172258608/).

# Updating your Nightscout for Push Notifications 

Your Nightscout website will generate push notifications and send them to this new mobile app for any events (Highs, Lows, Announcements, etc) that happen.  You can customize which types of notifications you want to receive when in the mobile application.  However, you need  to update your Azure website from this Work In Progress (WIP) branch of the [Nightscout repo](https://github.com/nightscout/cgm-remote-monitor/tree/wip/azurepush).

The steps to do this are:

1. Add the `azurepush` variable to your Enable string in application settings.
2. *IMPORTANT!* Ensure the `BASE_URL` is set in your App settings as described [here](https://github.com/srmoss/cgm-remote-monitor#required).  It should look something like this: `https://<your-website>.azurewebsties.net`.
3. Create a new branch on your fork in GitHub named wip/azurepush.  Do this by selecting the Branch drop down and typing in wip/azurepush and then selecting Create Branch.  More info [here](https://github.com/blog/1377-create-and-delete-branches).
4. Update your new wip/azurepush branch with the new code from the main repo.  You can create your own link to do this using this format: `https://github.com/nightscout/cgm-remote-monitor/compare/<your-github-username>:wip/azurepush...nightscout:wip/azurepush`.  Make sure you are logged in to GitHub first!
5. Approve and merge the pull request.
6. Go to Azure/Heroku and swap the branch that's deployed to your site.  You can either disconnect and re-connect, or follow these steps (for Azure):
> Go to your scm site `https://<your-website>.scm.azurewebsites.net`
> Go to the Debug Console - CMD from the top
> Navigate to site\deployments
> Edit the settings.xml file (using the little pencil)
> Change the value for the branch key from dev/master to wip/azurepush.  It should look like this:
```xml
<?xml version="1.0" encoding="utf-8"?>
<settings>
    <deployment>
        <add key="branch" value="wip/azurepush" />
    </deployment>
</settings>
```
7. Sync your deployment to move your site to the new code.  In Azure, go to your site, Deployment Options, select Sync.

And Voila!  The notification settings in the app will work now!
