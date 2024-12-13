# BuckshotPlusPlus (BPP) Ultimate Guide

## 1. Views (Components)

### Description
Views are the fundamental building blocks of BPP applications. They represent reusable UI components that can contain content, styling, and event handlers. Views are compiled into HTML elements and can be nested to create complex interfaces.

### Capabilities
- Define reusable UI components
- Apply styles and attributes
- Handle user interactions
- Nest and compose multiple components
- Reference other components or data sources

✅ DO:
```bpp
view Button {
    type = "button"
    content = "Click Me"
    background-color = "blue"
    color = "white"
    onclick = HandleClick
}

view Container {
    type = "div"
    content = [Header, MainContent, Footer]
}
```

❌ DON'T:
```bpp
view button {  // Wrong case
    Type = "button"  // Wrong case
    content="Click"  // Missing spaces around =
    onClick = () => {}  // No inline functions
}

view {  // Missing name
    type = "div"
}
```

## 2. Content Management

### Description
Content in BPP can be static text, references to other components, arrays of mixed content, or dynamic data from sources. Content is always managed as strings or arrays.

### Capabilities
- Static text content
- Component references
- Mixed content arrays
- Dynamic data binding
- String concatenation

✅ DO:
```bpp
content = "Static text"
content = OtherComponent
content = [Header, "Text", Footer]
content = DataSource.value
content = "Hello " + UserName.value + "!"
```

❌ DON'T:
```bpp
content = {text: "value"}  // No objects
content = <div>Text</div>  // No JSX
content = 42  // Must be string
content = ['item1',
           'item2']  // No multi-line arrays
```

## 3. Styling System

### Description
BPP supports all standard CSS properties using kebab-case notation. Styles can be applied directly to components and are compiled into actual CSS. All values must be strings.

### Capabilities
- Full CSS property support
- Layout control
- Visual styling
- Animations and transitions
- Responsive design

✅ DO:
```bpp
view StyledComponent {
    background-color = "#fff"
    margin = "20px"
    padding = "10px 20px"
    font-size = "16px"
    display = "flex"
    flex-direction = "column"
    box-shadow = "0 2px 4px rgba(0,0,0,0.1)"
}
```

❌ DON'T:
```bpp
view StyledComponent {
    backgroundColor = "white"  // No camelCase
    margin: "10px"  // Wrong syntax
    padding = 20  // Must be string
    font: "16px Arial"  // Use individual properties
    style = "color: red"  // No style attribute
}
```

## 3b. Input Types

### Description
BPP uses two distinct type attributes: `type` for HTML elements and `input-type` for input field types. This separation helps avoid confusion between element types and input types while maintaining clean, readable code.

### Capabilities
- Define HTML element types with `type`
- Specify input field types with `input-type`
- Support all standard HTML input types
- Clear separation of concerns
- Improved code readability

✅ DO:
```bpp
view TextInput {
    type = "input"
    input-type = "text"
    placeholder = "Enter text"
}

view PasswordField {
    type = "input"
    input-type = "password"
    placeholder = "Enter password"
}

view NumberInput {
    type = "input"
    input-type = "number"
    min = "0"
    max = "100"
}

view Form {
    type = "form"
    content = [
        EmailInput,
        SubmitButton
    ]
}
```
❌ DON'T:
```bpp
bppCopyview WrongInput {
    type = "text"  // Wrong: using input type as element type
    placeholder = "Wrong"
}

view IncorrectPassword {
    input-type = "password"  // Missing type="input"
    placeholder = "Wrong"
}

view MixedUp {
    type = "input"
    type = "password"  // Wrong: using type instead of input-type
}

view InvalidType {
    type = "textfield"  // Wrong: not a valid HTML element
    input-type = "text"
}
```
The following input types are supported through input-type:

text
password
number
email
tel
url
search
date
time
datetime-local
month
week
color
file
radio
checkbox
submit
reset
button

## 4. Event System

### Description
Events in BPP handle user interactions and state changes. They can modify component properties, update content, and trigger other actions. Events are defined separately and referenced by components.

### Capabilities
- Handle user interactions
- Update component properties
- Modify content
- Trigger state changes
- Form handling

✅ DO:
```bpp
event HandleClick {
    Button.content = "Clicked!"
    Button.background-color = "green"
    Status.content = "Processing..."
}

view Interactive {
    type = "button"
    onclick = HandleClick
    onmouseover = ShowTooltip
    onsubmit = ProcessForm
}
```

❌ DON'T:
```bpp
event HandleClick() {  // No parentheses
    return value;  // No returns
}

view Interactive {
    onclick = "handleClick()"  // No string handlers
    onclick = () => {}  // No inline functions
    OnClick = Handle  // Wrong case
}
```

## 5. Data Management

### Description
BPP provides several ways to manage data: data containers, sources for external APIs, and database connections. All data values are stored as strings.

### Capabilities
- Local data storage
- External API integration
- Database connections
- Environment variables
- Session data

✅ DO:
```bpp
data AppState {
    user = "John"
    theme = "dark"
    count = "42"
}

source API {
    type = "http"
    url = "https://api.example.com"
    method = "GET"
    headers = ["Auth: token"]
}

database DB {
    type = "mysql"
    Server = "localhost"
    Database = "app"
    UserId = process.env.DB_USER
}
```

❌ DON'T:
```bpp
data AppState {
    user = John  // Missing quotes
    count = 42  // Must be string
    items = [  // No multi-line arrays
        "item1",
        "item2"
    ]
}

source API {
    url = BaseUrl + "/api"  // No concatenation
    onSuccess = () => {}  // No functions
}
```

## 6. Pages and Routing

### Description
Pages represent complete web pages in BPP. They can include metadata, resources, and main content. Routes handle different URL paths in the application.

### Capabilities
- Define complete pages
- Include resources
- Set metadata
- Handle routes
- Configure SEO

✅ DO:
```bpp
page index {
    title = "Home"
    description = "Welcome"
    scripts = ["app.js"]
    css = ["style.css"]
    body = MainComponent
    icon = "favicon.ico"
}

route UserProfile {
    path = "/user"
    handler = ShowProfile
}
```

❌ DON'T:
```bpp
page index.html {  // No file extensions
    content = Main  // Wrong property
    script = app.js  // Missing quotes
}

route {  // Missing name
    url = "/page"  // Wrong property
    handle = Show  // Wrong property
}
```

## 7. Logic Control

### Description
BPP supports conditional rendering and basic logic control through if statements. All conditions must compare strings.

### Capabilities
- Conditional rendering
- Basic comparisons
- Component toggling
- State-based rendering

✅ DO:
```bpp
if(user.logged_in == "true") {
    view Dashboard {
        content = "Welcome"
    }
} else {
    view LoginForm {
        content = "Please log in"
    }
}
```

❌ DON'T:
```bpp
if user.logged_in {  // Missing parentheses
    content = "Hi"   // Must be in view
}

if(user.logged_in === true)  // No === operator
if(count > 0)  // Must compare strings
```

## 8. Session Management

### Description
BPP automatically manages user sessions and provides access to session data through a built-in session object. Sessions track user information and state across page loads.

### Capabilities
- Unique session IDs
- User tracking
- Visit counting
- Platform detection
- IP tracking
- Language detection

✅ DO:
```bpp
data UserSession {
    id = session.id
    lang = session.lang
    platform = session.platform
    visits = session.url_visited_num
}

view SessionInfo {
    type = "div"
    content = "Welcome back! Visit count: " + session.url_visited_num
}
```

❌ DON'T:
```bpp
data UserSession {
    session = getSession()  // No functions
    id = createId()        // No functions
    data = session         // Must access specific properties
}

session.id = "newId"  // Can't modify session directly
```

## 9. Analytics

### Description
BPP provides built-in analytics capabilities for tracking user interactions and events. Events are automatically timestamped and can include custom data.

### Capabilities
- Event tracking
- Page views
- User interactions
- Custom events
- Automatic timestamping

✅ DO:
```bpp
event TrackPageView {
    Analytics.event = "page_view"
    Analytics.page = "home"
    Analytics.custom_data = "user_type:premium"
}

view TrackedButton {
    type = "button"
    content = "Purchase"
    onclick = TrackPurchase
}

event TrackPurchase {
    Analytics.event = "purchase"
    Analytics.value = ProductPrice.value
}
```

❌ DON'T:
```bpp
Analytics.track()  // No method calls

event Track {
    Analytics.timestamp = Date.now()  // No JavaScript
    Analytics = {event: "view"}       // No objects
}
```

## 10. Security Features

### Description
BPP includes built-in security features for protecting data, sanitizing input, and managing sensitive information. Security features are automatically applied where appropriate.

### Capabilities
- Input sanitization
- XSS protection
- Environment variables
- Secure headers
- Session protection

✅ DO:
```bpp
data SecureConfig {
    api_key = process.env.API_KEY
    secret = process.env.APP_SECRET
}

view SecureInput {
    type = "input"
    type = "password"
    oninput = ValidateInput
}

event ValidateInput {
    if(Input.value == "<script>") {
        Input.value = ""
        Error.content = "Invalid input"
    }
}
```

❌ DON'T:
```bpp
data Secrets {
    key = "actual-api-key"  // No hardcoded secrets
    password = db.password  // No direct db access
}

event HandleInput {
    Output.content = Input.value  // Unsanitized input
}
```

## 11. Performance Optimization

### Description
BPP includes various performance optimization features and best practices for building efficient applications. Many optimizations are automatic.

### Capabilities
- Automatic resource optimization
- Component caching
- Lazy loading
- Memory management
- Response optimization

✅ DO:
```bpp
page optimized {
    scripts = ["defer:analytics.js"]
    css = ["critical.css"]
}

view EfficientList {
    type = "ul"
    content = DataSource.items
}

source CachedAPI {
    type = "http"
    url = "https://api.example.com"
    cache = "true"
}
```

❌ DON'T:
```bpp
view Inefficient {
    content = [
        Wrapper {
            content = MoreWrappers {  // Excessive nesting
                content = ActualContent
            }
        }
    ]
}

// Don't create unnecessary containers
view RedundantWrapper {
    type = "div"
    content = SingleChild
}
```

## 12. Environment Configuration

### Description
BPP provides environment configuration through .env files and the process.env object. This allows for different configurations in development and production.

### Capabilities
- Environment variables
- Configuration management
- Deployment settings
- Feature flags
- API configuration

✅ DO:
```bpp
data Config {
    env = process.env.NODE_ENV
    api_url = process.env.API_URL
    debug = process.env.DEBUG
}

source API {
    url = process.env.API_ENDPOINT
    key = process.env.API_KEY
}
```

❌ DON'T:
```bpp
data Config {
    env = getEnvironment()  // No functions
    url = "http://prod.api.com"  // No hardcoded values
}

process.env.API_KEY = "new-key"  // Can't modify env vars
```

## 13. Project Structure and Organization

### Description
BPP has recommended project structure patterns and organization principles for maintainable applications.

### Capabilities
- Modular organization
- Component separation
- Resource management
- Code splitting
- File organization

✅ DO:
```bpp
// main.bpp
include "components/Header.bpp"
include "components/Footer.bpp"
include "pages/Home.bpp"

// components/Header.bpp
view Header {
    type = "header"
    content = [Logo, Navigation]
}
```

❌ DON'T:
```bpp
// Don't mix unrelated components in one file
view Header { /* ... */ }
view Footer { /* ... */ }
view Sidebar { /* ... */ }

include "components/" + type  // No dynamic includes
```

## 14. Development Tools

### Description
BPP provides various development tools and commands for building, testing, and deploying applications.

### Capabilities
- Development server
- Static export
- File merging
- Error reporting
- Build optimization

✅ DO:
```bash
# Start development server
bpp main.bpp

# Export static site
bpp export main.bpp ./dist

# Merge includes
bpp merge main.bpp
```

❌ DON'T:
```bash
bpp run main  # Wrong command
bpp compile   # Unsupported command
bpp watch     # Unsupported command
```

## 15. Advanced Features

### Description
BPP includes several advanced features for complex applications and special use cases.

### Capabilities
- Custom attributes
- Meta programming
- Special handlers
- System integration
- Advanced routing

✅ DO:
```bpp
view Advanced {
    type = "div"
    data-testid = "unique-id"
    aria-label = "Accessible name"
    role = "button"
}

meta SEO {
    name = "description"
    content = "Page description"
}
```

❌ DON'T:
```bpp
view Advanced {
    @click = Handler  // No special characters
    $ref = "myRef"    // No special characters
    custom:attr = "value"  // No custom syntax
}
```

Remember these key principles:
1. All values must be strings
2. No inline functions or computations
3. No comments in code
4. Keep components focused and single-purpose
5. Use proper case for different elements:
    - PascalCase for components and events
    - kebab-case for CSS properties
    - lowercase for HTML attributes
6. Always include spaces around = operator
7. Arrays must be single-line
8. Security first - validate all inputs
9. Performance matters - optimize component structure
10. Maintain clean project organization

This guide covers the complete feature set of BPP. Use it as a reference when implementing features and helping users write correct, efficient BPP code.